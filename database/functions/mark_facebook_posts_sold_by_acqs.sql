CREATE OR REPLACE FUNCTION public.mark_facebook_posts_sold_by_acq(p_acq_id integer, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_rows INT;
BEGIN
    UPDATE facebook_posts
    SET
        mark_as_sold = TRUE,
        updated_at = CURRENT_TIMESTAMP
    WHERE acq_id = p_acq_id
      AND mark_as_sold = FALSE;

    GET DIAGNOSTICS v_rows = ROW_COUNT;

    IF v_rows > 0 THEN
        INSERT INTO data_entry_log (
            table_name,
            record_id,
            action_type,
            entered_by,
            change_details
        )
        VALUES (
            'facebook_posts',
            p_acq_id,
            'UPDATE',
            p_entered_by,
            jsonb_build_object(
                'action', 'auto_mark_sold_from_sale',
                'acq_id', p_acq_id,
                'rows_updated', v_rows
            )
        );
    END IF;

    RETURN v_rows;
END;
$function$
;
