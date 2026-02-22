CREATE OR REPLACE FUNCTION public.insert_sale(p_item_id integer, p_date_sold timestamp with time zone, p_qty_sold integer, p_channel_type character varying, p_booth_id integer, p_unit_sale_price numeric, p_discounted_rate numeric, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_sale_id integer;
BEGIN
    INSERT INTO public.sales (
        item_id,
        date_sold,
        qty_sold,
        channel_type,
        booth_id,
        unit_sale_price,
        discounted_rate
    )
    VALUES (
        p_item_id,
        p_date_sold,
        COALESCE(p_qty_sold, 1),
        p_channel_type,
        p_booth_id,
        p_unit_sale_price,
        p_discounted_rate
    )
    RETURNING sale_id INTO v_sale_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'sales',
        v_sale_id,
        'INSERT',
        p_entered_by,
        jsonb_build_object(
            'item_id', p_item_id,
            'qty_sold', p_qty_sold,
            'unit_sale_price', p_unit_sale_price
        )
    );

    RETURN v_sale_id;
END;
$function$
;
