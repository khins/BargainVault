CREATE OR REPLACE FUNCTION public.delete_facebook_post(p_post_id integer, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_acq_id   int;
    v_item_id  int;
    v_title    text;
BEGIN
    -- Capture related acquisition + item info BEFORE delete
    SELECT
        a.acq_id,
        i.item_id,
        i.title
    INTO
        v_acq_id,
        v_item_id,
        v_title
    FROM public.facebook_posts fp
    JOIN public.acquisitions a ON a.acq_id = fp.acq_id
    JOIN public.items i ON i.item_id = a.item_id
    WHERE fp.post_id = p_post_id;

   	IF v_acq_id IS NULL THEN
    	RAISE EXCEPTION 'Facebook post % not found', p_post_id;
	END IF;
   
    -- Delete facebook post
    DELETE FROM public.facebook_posts
    WHERE post_id = p_post_id;

    -- Log delete with full context
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
        'DELETE',
        p_entered_by,
        jsonb_build_object(
            'post_id', p_post_id,
            'acq_id', v_acq_id,
            'item_id', v_item_id,
            'item_title', v_title
        )
    );

    RETURN p_post_id;
END;
$function$
;
