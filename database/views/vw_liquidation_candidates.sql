CREATE OR REPLACE VIEW public.vw_liquidation_candidates AS
WITH latest_acq AS (
    SELECT DISTINCT ON (a.item_id)
        a.item_id,
        a.acq_id,
        a.total_settlement,
        a.date_acquired::date AS date_acquired
    FROM acquisitions a
    ORDER BY a.item_id, a.date_acquired DESC NULLS LAST
),
latest_sale AS (
    SELECT DISTINCT ON (s.item_id)
        s.item_id,
        s.date_sold::date AS date_sold
    FROM sales s
    ORDER BY s.item_id, s.date_sold DESC NULLS LAST
),
latest_retail AS (
    SELECT DISTINCT ON (rp.item_id)
        rp.item_id,
        rp.retail_price
    FROM retail_prices rp
    ORDER BY rp.item_id, rp.price_date DESC NULLS LAST
),
latest_fb AS (
    SELECT DISTINCT ON (a.item_id)
        a.item_id,
        f.post_date::date AS post_date,
        f.asking_price
    FROM facebook_posts f
    JOIN acquisitions a ON a.acq_id = f.acq_id
    ORDER BY a.item_id, f.post_date DESC NULLS LAST
)

SELECT
    i.item_id,
    i.title,
    i.created_at::date AS created_at,

    la.acq_id,
    la.total_settlement,
    la.date_acquired,

    lr.retail_price,
    lf.post_date AS last_post_date,
    lf.asking_price AS last_asking_price,

    -- ✅ SAFE days in inventory calculation
    (
        CURRENT_DATE -
        COALESCE(la.date_acquired, i.created_at::date)
    ) AS days_in_inventory,

    -- Auction estimate
    CASE
        WHEN lr.retail_price IS NOT NULL THEN lr.retail_price * 0.35
        WHEN la.total_settlement IS NOT NULL THEN la.total_settlement * 0.75
        ELSE NULL
    END AS auction_estimate,

    -- Simple liquidation score
    (
        (CURRENT_DATE - COALESCE(la.date_acquired, i.created_at::date))
        +
        CASE WHEN lf.post_date IS NULL THEN 30 ELSE 0 END
    ) AS ile_score,

    CASE
        WHEN (CURRENT_DATE - COALESCE(la.date_acquired, i.created_at::date)) > 180 THEN 'LIQUIDATE'
        WHEN (CURRENT_DATE - COALESCE(la.date_acquired, i.created_at::date)) > 90 THEN 'DISCOUNT'
        ELSE 'HOLD'
    END AS recommendation

FROM items i
LEFT JOIN latest_acq la ON la.item_id = i.item_id
LEFT JOIN latest_retail lr ON lr.item_id = i.item_id
LEFT JOIN latest_fb lf ON lf.item_id = i.item_id
LEFT JOIN latest_sale ls ON ls.item_id = i.item_id

-- only unsold items
WHERE ls.item_id IS NULL;