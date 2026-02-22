CREATE OR REPLACE FUNCTION public.update_acquisition(p_acq_id integer, p_item_id integer, p_source_type text, p_auction_site_id integer, p_date_acquired timestamp without time zone, p_qty_acquired integer, p_unit_hammer_price numeric, p_buyer_premium numeric, p_tax_rate numeric, p_sales_tax_paid numeric, p_total_settlement numeric, p_status_id integer, p_personal boolean, p_business_expense boolean, p_entered_by text)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE acquisitions
    SET
        item_id            = p_item_id,
        source_type         = p_source_type,
        auction_site_id     = p_auction_site_id,
        date_acquired       = p_date_acquired,
        qty_acquired        = p_qty_acquired,
        unit_hammer_price   = p_unit_hammer_price,
        buyer_premium       = p_buyer_premium,
        tax_rate            = p_tax_rate,
        sales_tax_paid      = p_sales_tax_paid,
        total_settlement    = p_total_settlement,
        status_id           = p_status_id,
        personal            = p_personal,
        business_expense    = p_business_expense
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
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'item_id', p_item_id,
            'total_settlement', p_total_settlement
        )
    );
END;
$function$
;
