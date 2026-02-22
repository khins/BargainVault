-- public.auction_sites definition

-- Drop table

-- DROP TABLE public.auction_sites;

CREATE TABLE public.auction_sites (
	auction_site_id serial4 NOT NULL,
	"name" varchar(255) NOT NULL,
	address varchar(255) NULL,
	city varchar(100) NULL,
	tax_rate numeric(6, 4) NULL,
	buyer_premium numeric(12, 2) NULL,
	date_created timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	state varchar(2) NULL,
	CONSTRAINT auction_sites_pkey PRIMARY KEY (auction_site_id)
);