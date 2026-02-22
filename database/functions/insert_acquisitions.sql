CREATE OR REPLACE FUNCTION public.insert_acquisition(p_item_id integer, p_source_type character varying, p_auction_site_id integer, p_date_acquired timestamp without time zone, p_qty_acquired integer, p_unit_hammer_price numeric, p_buyer_premium numeric, p_tax_rate numeric, p_sales_tax_paid numeric, p_total_settlement numeric, p_status_id integer, p_personal boolean, p_business_expense boolean, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_acq_id int;
    v_new_row jsonb;
BEGIN
    INSERT INTO public.acquisitions (
        item_id,
        source_type,
        auction_site_id,
        date_acquired,
        qty_acquired,
        unit_hammer_price,
        buyer_premium,
        tax_rate,
        sales_tax_paid,
        total_settlement,
        status_id,
        personal,
        business_expense
    )
    VALUES (
        p_item_id,
        p_source_type,
        p_auction_site_id,
        p_date_acquired ,
        COALESCE(p_qty_acquired, 1),
        p_unit_hammer_price,
        p_buyer_premium,
        p_tax_rate,
        p_sales_tax_paid,
        p_total_settlement,
        p_status_id,
        COALESCE(p_personal, false),
        COALESCE(p_business_expense, false)
    )
    RETURNING acq_id INTO v_acq_id;

    SELECT to_jsonb(a)
    INTO v_new_row
    FROM public.acquisitions a
    WHERE a.acq_id = v_acq_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'acquisitions',
        v_acq_id,
        'INSERT',
        p_entered_by,
        v_new_row
    );

    RETURN v_acq_id;
END;
$function$
;
