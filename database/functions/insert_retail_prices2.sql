CREATE OR REPLACE FUNCTION public.insert_retail_price(p_item_id integer, p_store_id integer, p_price numeric, p_price_date date, p_is_sale boolean, p_notes text, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_new_id int;
BEGIN
    INSERT INTO retail_prices (
        item_id,
        store_id,
        retail_price,
        price_date,
        is_sale_price,
        notes
    )
    VALUES (
        p_item_id,
        p_store_id,
        p_price,
        p_price_date,
        p_is_sale,
        p_notes
    )
    RETURNING retail_price_id
    INTO v_new_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'retail_prices',
        v_new_id,
        'INSERT',
        p_entered_by,
        jsonb_build_object(
            'item_id', p_item_id,
            'store_id', p_store_id,
            'price', p_price,
            'price_date', p_price_date
        )
    );

    RETURN v_new_id;
END;
$function$
;
