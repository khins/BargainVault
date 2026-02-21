-- public.booths definition

-- Drop table

-- DROP TABLE public.booths;

CREATE TABLE public.booths (
	booth_id serial4 NOT NULL,
	booth_name varchar(150) NOT NULL,
	commission_rate numeric(6, 4) NULL,
	rent_per_period numeric(12, 2) NULL,
	created_at timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	CONSTRAINT booths_pkey PRIMARY KEY (booth_id)
);