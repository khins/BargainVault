CREATE OR REPLACE FUNCTION public.delete_inventory_location_by_item(p_item_id integer, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_inventory_location_id int;
BEGIN
    SELECT inventory_location_id
    INTO v_inventory_location_id
    FROM inventory_locations
    WHERE item_id = p_item_id
    LIMIT 1;

    IF v_inventory_location_id IS NULL THEN
        RETURN;
    END IF;

    DELETE FROM inventory_locations
    WHERE inventory_location_id = v_inventory_location_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by
    )
    VALUES (
        'inventory_locations',
        v_inventory_location_id,
        'DELETE',
        p_entered_by
    );
END;
$function$
;
