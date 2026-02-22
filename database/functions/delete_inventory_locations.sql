CREATE OR REPLACE FUNCTION public.delete_inventory_location(p_inventory_location_id integer, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
BEGIN
    DELETE FROM public.inventory_locations
    WHERE inventory_location_id = p_inventory_location_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by
    )
    VALUES (
        'inventory_locations',
        p_inventory_location_id,
        'DELETE',
        p_entered_by
    );

    RETURN p_inventory_location_id;
END;
$function$
;
