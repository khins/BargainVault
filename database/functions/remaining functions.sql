CREATE OR REPLACE FUNCTION public.update_facebook_post(p_post_id integer, p_post_date timestamp without time zone, p_post_title character varying, p_post_description text, p_asking_price numeric, p_boosted boolean, p_mark_as_sold boolean, p_renew_date timestamp without time zone, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE public.facebook_posts
    SET
        post_date = p_post_date,
        post_title = p_post_title,
        post_description = p_post_description,
        asking_price = p_asking_price,
        boosted = COALESCE(p_boosted, boosted),
        mark_as_sold = COALESCE(p_mark_as_sold, mark_as_sold),
        renew_date = p_renew_date,
        updated_at = CURRENT_TIMESTAMP
    WHERE post_id = p_post_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'facebook_posts',
        p_post_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'asking_price', p_asking_price,
            'boosted', p_boosted,
            'mark_as_sold', p_mark_as_sold
        )
    );

    RETURN p_post_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_inventory_location(p_inventory_location_id integer, p_item_id integer, p_booth_id integer, p_status_id integer, p_date_placed timestamp without time zone, p_asking_price numeric, p_notes text, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE public.inventory_locations
    SET
        item_id = p_item_id,
        booth_id = p_booth_id,
        status_id = p_status_id,
        date_placed = p_date_placed,
        asking_price = p_asking_price,
        notes = p_notes,
        updated_at = CURRENT_TIMESTAMP
    WHERE inventory_location_id = p_inventory_location_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'inventory_locations',
        p_inventory_location_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'item_id', p_item_id,
            'booth_id', p_booth_id,
            'status_id', p_status_id,
            'date_placed', p_date_placed,
            'asking_price', p_asking_price
        )
    );

    RETURN p_inventory_location_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_inventory_location(p_inventory_location_id integer, p_item_id integer, p_booth_id integer, p_status_id integer, p_date_placed timestamp with time zone, p_asking_price numeric, p_notes text, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE public.inventory_locations
    SET
        item_id = p_item_id,
        booth_id = p_booth_id,
        status_id = p_status_id,
        date_placed = p_date_placed,
        asking_price = p_asking_price,
        notes = p_notes,
        updated_at = CURRENT_TIMESTAMP
    WHERE inventory_location_id = p_inventory_location_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'inventory_locations',
        p_inventory_location_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'item_id', p_item_id,
            'booth_id', p_booth_id,
            'status_id', p_status_id,
            'date_placed', p_date_placed,
            'asking_price', p_asking_price
        )
    );

    RETURN p_inventory_location_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_item(p_item_id integer, p_lot_number integer, p_title character varying, p_description text, p_image_path character varying, p_quantity integer, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_old RECORD;
BEGIN
    SELECT *
    INTO v_old
    FROM public.items
    WHERE item_id = p_item_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'Item % not found', p_item_id;
    END IF;

    UPDATE public.items
    SET
        lot_number  = p_lot_number,
        title       = p_title,
        description = p_description,
        image_path  = p_image_path,
        quantity    = p_quantity
    WHERE item_id = p_item_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'items',
        p_item_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'old', row_to_json(v_old),
            'new', jsonb_build_object(
                'lot_number', p_lot_number,
                'title', p_title,
                'description', p_description,
                'image_path', p_image_path,
                'quantity', p_quantity
            )
        )
    );
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_retail_price(p_retail_price_id integer, p_item_id integer, p_store_id integer, p_retail_price numeric, p_price_date timestamp with time zone, p_is_sale_price boolean, p_notes text, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_old jsonb;
BEGIN
    SELECT jsonb_build_object(
        'item_id', item_id,
        'store_id', store_id,
        'retail_price', retail_price,
        'price_date', price_date,
        'is_sale_price', is_sale_price,
        'notes', notes
    )
    INTO v_old
    FROM retail_prices
    WHERE retail_price_id = p_retail_price_id;

    UPDATE public.retail_prices
    SET
        item_id = p_item_id,
        store_id = p_store_id,
        retail_price = p_retail_price,
        price_date = p_price_date,
        is_sale_price = COALESCE(p_is_sale_price, false),
        notes = p_notes
    WHERE retail_price_id = p_retail_price_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'retail_prices',
        p_retail_price_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'before', v_old,
            'after', jsonb_build_object(
                'item_id', p_item_id,
                'store_id', p_store_id,
                'retail_price', p_retail_price,
                'price_date', p_price_date,
                'is_sale_price', p_is_sale_price,
                'notes', p_notes
            )
        )
    );
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_retail_price(p_retail_price_id integer, p_item_id integer, p_store_id integer, p_retail_price numeric, p_price_date timestamp without time zone, p_is_sale_price boolean, p_notes text, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_old jsonb;
BEGIN
    SELECT jsonb_build_object(
        'item_id', item_id,
        'store_id', store_id,
        'retail_price', retail_price,
        'price_date', price_date,
        'is_sale_price', is_sale_price,
        'notes', notes
    )
    INTO v_old
    FROM retail_prices
    WHERE retail_price_id = p_retail_price_id;

    UPDATE public.retail_prices
    SET
        item_id = p_item_id,
        store_id = p_store_id,
        retail_price = p_retail_price,
        price_date = p_price_date,
        is_sale_price = COALESCE(p_is_sale_price, false),
        notes = p_notes,
        updated_at = CURRENT_TIMESTAMP
    WHERE retail_price_id = p_retail_price_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'retail_prices',
        p_retail_price_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'before', v_old,
            'after', jsonb_build_object(
                'item_id', p_item_id,
                'store_id', p_store_id,
                'retail_price', p_retail_price,
                'price_date', p_price_date,
                'is_sale_price', p_is_sale_price,
                'notes', p_notes
            )
        )
    );
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_sale(p_sale_id integer, p_item_id integer, p_date_sold timestamp with time zone, p_qty_sold integer, p_channel_type character varying, p_booth_id integer, p_unit_sale_price numeric, p_discounted_rate numeric, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE public.sales
    SET
        item_id           = p_item_id,
        date_sold         = p_date_sold,
        qty_sold          = p_qty_sold,
        channel_type      = p_channel_type,
        booth_id          = p_booth_id,
        unit_sale_price   = p_unit_sale_price,
        discounted_rate   = p_discounted_rate
    WHERE sale_id = p_sale_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'sales',
        p_sale_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'qty_sold', p_qty_sold,
            'unit_sale_price', p_unit_sale_price,
            'discounted_rate', p_discounted_rate
        )
    );
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_sale(p_sale_id integer, p_item_id integer, p_date_sold timestamp without time zone, p_qty_sold integer, p_channel_type character varying, p_booth_id integer, p_unit_sale_price numeric, p_discounted_rate numeric, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE public.sales
    SET
        item_id           = p_item_id,
        date_sold         = p_date_sold,
        qty_sold          = p_qty_sold,
        channel_type      = p_channel_type,
        booth_id          = p_booth_id,
        unit_sale_price   = p_unit_sale_price,
        discounted_rate   = p_discounted_rate
    WHERE sale_id = p_sale_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'sales',
        p_sale_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'qty_sold', p_qty_sold,
            'unit_sale_price', p_unit_sale_price,
            'discounted_rate', p_discounted_rate
        )
    );
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_timestamp()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$function$
;
