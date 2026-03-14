CREATE VIEW public.vw_liquidation_candidates AS
WITH latest_retail AS (
    SELECT DISTINCT ON (rp.item_id)
        rp.item_id,
        rp.retail_price
    FROM retail_prices rp
    ORDER BY rp.item_id, rp.price_date DESC NULLS LAST
),

latest_fb AS (
    SELECT DISTINCT ON (a.item_id)
        a.item_id,
        f.post_date,
        f.asking_price
    FROM facebook_posts f
    JOIN acquisitions a ON a.acq_id = f.acq_id
    ORDER BY a.item_id, f.post_date DESC NULLS LAST
),

latest_acq AS (
    SELECT DISTINCT ON (a.item_id)
        a.item_id,
        a.acq_id,
        a.total_settlement,
        a.date_acquired
    FROM acquisitions a
    ORDER BY a.item_id, a.date_acquired DESC NULLS LAST
)

SELECT
    i.item_id,
    i.title,
    i.created_at,
    i.image_path,
    i.disregard,

    la.acq_id,
    la.total_settlement,
    la.date_acquired,

    lr.retail_price,

    lf.post_date AS last_post_date,
    lf.asking_price AS last_asking_price,

    -- Days in inventory
    (CURRENT_DATE - COALESCE(la.date_acquired, i.created_at::date))::int
        AS days_in_inventory,

    -- Auction likelihood %
    CASE
        WHEN i.title ILIKE '%tool%' THEN 0.50
        WHEN i.title ILIKE '%electronics%' THEN 0.40
        WHEN i.title ILIKE '%dvd%' THEN 0.20
        WHEN i.title ILIKE '%toy%' THEN 0.30
        WHEN lr.retail_price IS NOT NULL THEN 0.35
        ELSE 0.25
    END AS auction_pct,

    -- Auction estimate
    CASE
        WHEN lr.retail_price IS NOT NULL THEN
            lr.retail_price *
            CASE
                WHEN i.title ILIKE '%tool%' THEN 0.50
                WHEN i.title ILIKE '%electronics%' THEN 0.40
                WHEN i.title ILIKE '%dvd%' THEN 0.20
                WHEN i.title ILIKE '%toy%' THEN 0.30
                ELSE 0.35
            END
        WHEN la.total_settlement IS NOT NULL THEN
            la.total_settlement * 0.75
        ELSE NULL
    END AS auction_estimate,

    -- ILE score
    (
        (CURRENT_DATE - COALESCE(la.date_acquired, i.created_at::date))
        +
        CASE WHEN lf.post_date IS NULL THEN 30 ELSE 0 END
        +
        CASE WHEN lf.post_date < CURRENT_DATE - INTERVAL '30 days' THEN 20 ELSE 0 END
    )::int AS ile_score,

    -- Recommendation
    CASE
        WHEN (CURRENT_DATE - COALESCE(la.date_acquired, i.created_at::date)) > 180
            THEN 'LIQUIDATE'
        WHEN (CURRENT_DATE - COALESCE(la.date_acquired, i.created_at::date)) > 90
            THEN 'DISCOUNT'
        ELSE 'HOLD'
    END AS recommendation

FROM items i
LEFT JOIN latest_acq la ON la.item_id = i.item_id
LEFT JOIN latest_retail lr ON lr.item_id = i.item_id
LEFT JOIN latest_fb lf ON lf.item_id = i.item_id

WHERE COALESCE(i.disregard,false) = false
AND NOT EXISTS (
    SELECT 1
    FROM sales s
    WHERE s.item_id = i.item_id
);
