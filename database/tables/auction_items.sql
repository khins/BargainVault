-- public.auction_items definition

-- Drop table

-- DROP TABLE public.auction_items;

CREATE TABLE public.auction_items (
	auction_item_id serial4 NOT NULL,
	item_name varchar(255) NOT NULL,
	description text NULL,
	created_at timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	auction_site_id int4 NULL,
	staged_for_auction bool NULL DEFAULT false,
	staged_date timestamp NULL,
	CONSTRAINT auction_items_pkey PRIMARY KEY (auction_item_id)
);


-- public.auction_items foreign keys

ALTER TABLE public.auction_items ADD CONSTRAINT fk_auctionitems_site FOREIGN KEY (auction_site_id) REFERENCES public.auction_sites(auction_site_id);