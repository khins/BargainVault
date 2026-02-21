-- public.sales definition

-- Drop table

-- DROP TABLE public.sales;

CREATE TABLE public.sales (
	sale_id serial4 NOT NULL,
	item_id int4 NOT NULL,
	date_sold date NOT NULL,
	qty_sold int4 NOT NULL DEFAULT 1,
	channel_type varchar(50) NULL,
	booth_id int4 NULL,
	unit_sale_price numeric(10, 2) NOT NULL,
	discounted_rate numeric(10, 2) NULL,
	CONSTRAINT sales_pkey PRIMARY KEY (sale_id)
);


-- public.sales foreign keys

ALTER TABLE public.sales ADD CONSTRAINT fk_sales_booth FOREIGN KEY (booth_id) REFERENCES public.booths(booth_id);
ALTER TABLE public.sales ADD CONSTRAINT fk_sales_item FOREIGN KEY (item_id) REFERENCES public.items(item_id);