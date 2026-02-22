-- public.inventory_status definition

-- Drop table

-- DROP TABLE public.inventory_status;

CREATE TABLE public.inventory_status (
	status_id serial4 NOT NULL,
	status_name varchar(100) NOT NULL,
	CONSTRAINT inventory_status_pkey PRIMARY KEY (status_id)
);