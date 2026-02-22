CREATE OR REPLACE FUNCTION public.insert_facebook_post(p_acq_id integer, p_post_date timestamp without time zone, p_post_title character varying, p_post_description text, p_asking_price numeric, p_boosted boolean, p_mark_as_sold boolean, p_renew_date timestamp without time zone, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_post_id int;
BEGIN
    INSERT INTO public.facebook_posts (
        acq_id,
        post_date,
        post_title,
        post_description,
        asking_price,
        boosted,
        mark_as_sold,
        renew_date
    )
    VALUES (
        p_acq_id,
        p_post_date,
        p_post_title,
        p_post_description,
        p_asking_price,
        COALESCE(p_boosted, false),
        COALESCE(p_mark_as_sold, false),
        p_renew_date
    )
    RETURNING post_id INTO v_post_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'facebook_posts',
        v_post_id,
        'INSERT',
        p_entered_by,
        jsonb_build_object(
            'acq_id', p_acq_id,
            'asking_price', p_asking_price,
            'boosted', p_boosted
        )
    );

    RETURN v_post_id;
END;
$function$
;
