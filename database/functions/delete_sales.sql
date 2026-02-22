CREATE OR REPLACE FUNCTION public.delete_sale(p_sale_id integer, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    DELETE FROM public.sales
    WHERE sale_id = p_sale_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by
    )
    VALUES (
        'sales',
        p_sale_id,
        'DELETE',
        p_entered_by
    );
END;
$function$
;
