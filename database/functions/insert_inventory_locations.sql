CREATE OR REPLACE FUNCTION public.insert_inventory_location(p_item_id integer, p_booth_id integer, p_status_id integer, p_date_placed timestamp with time zone, p_asking_price numeric, p_notes text, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_inventory_location_id int;
BEGIN
    INSERT INTO public.inventory_locations (
        item_id,
        booth_id,
        status_id,
        date_placed,
        asking_price,
        notes
    )
    VALUES (
        p_item_id,
        p_booth_id,
        p_status_id,
        p_date_placed,
        p_asking_price,
        p_notes
    )
    RETURNING inventory_location_id
    INTO v_inventory_location_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'inventory_locations',
        v_inventory_location_id,
        'INSERT',
        p_entered_by,
        jsonb_build_object(
            'item_id', p_item_id,
            'booth_id', p_booth_id,
            'status_id', p_status_id,
            'date_placed', p_date_placed,
            'asking_price', p_asking_price
        )
    );

    RETURN v_inventory_location_id;
END;
$function$
;
