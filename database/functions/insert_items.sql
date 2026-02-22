CREATE OR REPLACE FUNCTION public.insert_item(p_lot_number integer, p_title character varying, p_description text, p_image_path character varying, p_quantity integer, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_item_id INT;
BEGIN
    -- Basic validation
    IF p_title IS NULL OR length(trim(p_title)) = 0 THEN
        RAISE EXCEPTION 'Title is required';
    END IF;

    -- Insert item
    INSERT INTO public.items (
        lot_number,
        title,
        description,
        image_path,
        quantity
    )
    VALUES (
        p_lot_number,
        p_title,
        p_description,
        p_image_path,
        COALESCE(p_quantity, 1)
    )
    RETURNING item_id INTO v_item_id;

    -- Log entry
    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'items',
        v_item_id,
        'INSERT',
        p_entered_by,
        jsonb_build_object(
            'lot_number', p_lot_number,
            'title', p_title,
            'description', p_description,
            'image_path', p_image_path,
            'quantity', COALESCE(p_quantity, 1)
        )
    );

    RETURN v_item_id;
END;
$function$
;
