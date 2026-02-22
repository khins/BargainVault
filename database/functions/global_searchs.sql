CREATE OR REPLACE FUNCTION public.global_search(p_search text)
 RETURNS TABLE(entity_type text, entity_id integer, display_text text, secondary_text text)
 LANGUAGE plpgsql
AS $function$
BEGIN
RETURN QUERY

-- Items
SELECT
    'Item'::text,
    i.item_id,
    i.title::text,
    NULL::text
FROM items i
WHERE i.title ILIKE '%' || p_search || '%'

UNION ALL

-- Acquisitions
SELECT
    'Acquisition'::text,
    a.acq_id,
    i.title::text,
    ('Acq #' || a.acq_id)::text
FROM acquisitions a
JOIN items i ON i.item_id = a.item_id
WHERE i.title ILIKE '%' || p_search || '%'

UNION ALL

-- Facebook Posts
SELECT
    'FacebookPost'::text,
    f.post_id,
    f.post_title::text,
    i.title::text
FROM facebook_posts f
JOIN acquisitions a ON a.acq_id = f.acq_id
JOIN items i ON i.item_id = a.item_id
WHERE f.post_title ILIKE '%' || p_search || '%';


END;
$function$
;
