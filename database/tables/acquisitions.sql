-- public.acquisitions definition

-- Drop table

-- DROP TABLE public.acquisitions;

CREATE TABLE public.acquisitions (
	acq_id serial4 NOT NULL,
	item_id int4 NOT NULL,
	source_type varchar(50) NULL,
	auction_site_id int4 NULL,
	date_acquired date NULL,
	qty_acquired int4 NULL DEFAULT 1,
	unit_hammer_price numeric(12, 2) NULL,
	buyer_premium numeric(12, 2) NULL,
	sales_tax_paid numeric(12, 2) NULL,
	total_settlement numeric(12, 2) NULL,
	status_id int4 NULL,
	tax_rate numeric(6, 4) NULL,
	sold bool NULL DEFAULT false,
	sale_id int4 NULL,
	personal bool NULL DEFAULT false,
	business_expense bool NULL DEFAULT false,
	CONSTRAINT acquisitions_pkey PRIMARY KEY (acq_id)
);


-- public.acquisitions foreign keys

ALTER TABLE public.acquisitions ADD CONSTRAINT fk_acq_item FOREIGN KEY (item_id) REFERENCES public.items(item_id);
ALTER TABLE public.acquisitions ADD CONSTRAINT fk_acq_site FOREIGN KEY (auction_site_id) REFERENCES public.auction_sites(auction_site_id);