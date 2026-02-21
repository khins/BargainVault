-- public.stores definition

-- Drop table

-- DROP TABLE public.stores;

CREATE TABLE public.stores (
	store_id serial4 NOT NULL,
	store_name varchar(150) NOT NULL,
	store_type varchar(50) NULL,
	website_url varchar(255) NULL,
	created_at timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
	CONSTRAINT stores_pkey PRIMARY KEY (store_id),
	CONSTRAINT stores_store_name_uk UNIQUE (store_name)
);