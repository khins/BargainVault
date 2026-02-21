-- public.retail_prices definition

-- Drop table

-- DROP TABLE public.retail_prices;

CREATE TABLE public.retail_prices (
	retail_price_id serial4 NOT NULL,
	item_id int4 NOT NULL,
	retail_price numeric(12, 2) NULL,
	url_item_location text NULL,
	created_at timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	asin varchar(50) NULL,
	store_id int4 NULL,
	price_date timestamp NULL,
	is_sale_price bool NOT NULL DEFAULT false,
	notes text NULL,
	CONSTRAINT retail_prices_pkey PRIMARY KEY (retail_price_id)
);


-- public.retail_prices foreign keys

ALTER TABLE public.retail_prices ADD CONSTRAINT fk_retailprices_item FOREIGN KEY (item_id) REFERENCES public.items(item_id);
ALTER TABLE public.retail_prices ADD CONSTRAINT retail_prices_store_fk FOREIGN KEY (store_id) REFERENCES public.stores(store_id) ON DELETE RESTRICT;