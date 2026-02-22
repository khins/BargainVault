CREATE OR REPLACE FUNCTION public.delete_acquisition(p_acq_id integer, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_old_row jsonb;
BEGIN
    SELECT to_jsonb(a)
    INTO v_old_row
    FROM public.acquisitions a
    WHERE a.acq_id = p_acq_id;

    IF v_old_row IS NULL THEN
        RAISE EXCEPTION 'Acquisition % not found', p_acq_id;
    END IF;

    DELETE FROM public.acquisitions
    WHERE acq_id = p_acq_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'acquisitions',
        p_acq_id,
        'DELETE',
        p_entered_by,
        v_old_row
    );
END;
$function$
;
