-- public.items definition

-- Drop table

-- DROP TABLE public.items;

CREATE TABLE public.items (
	item_id int4 NOT NULL GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE),
	lot_number int4 NULL,
	title varchar(255) NOT NULL,
	description text NULL,
	image_path varchar(500) NULL,
	quantity int4 NULL DEFAULT 1,
	created_at timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	CONSTRAINT items_pkey PRIMARY KEY (item_id)
);