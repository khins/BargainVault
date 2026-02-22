-- public.vw_auction_profit_analysis source

CREATE OR REPLACE VIEW public.vw_auction_profit_analysis
AS SELECT a.acq_id,
    i.title,
    i.lot_number,
    s.name AS auction_site,
    a.total_settlement,
    a.total_settlement * 1.55 AS suggested_booth_price,
    a.total_settlement * 1.55 - a.total_settlement AS potential_profit
   FROM acquisitions a
     JOIN items i ON i.item_id = a.item_id
     JOIN auction_sites s ON s.auction_site_id = a.auction_site_id
  WHERE i.title::text !~~* '%card%'::text AND a.personal = false AND a.business_expense = false;