CREATE OR REPLACE FUNCTION public.mark_facebook_posts_sold_by_item(p_item_id integer, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_rows INT;
BEGIN
    UPDATE facebook_posts fp
    SET
        mark_as_sold = TRUE,
        updated_at = CURRENT_TIMESTAMP
    FROM acquisitions a
    WHERE fp.acq_id = a.acq_id
      AND a.item_id = p_item_id
      AND fp.mark_as_sold = FALSE;

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
            p_item_id,
            'UPDATE',
            p_entered_by,
            jsonb_build_object(
                'action', 'auto_mark_sold_from_sale',
                'item_id', p_item_id,
                'rows_updated', v_rows
            )
        );
    END IF;

    RETURN v_rows;
END;
$function$
;
