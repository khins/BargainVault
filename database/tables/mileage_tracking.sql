-- public.mileage_tracking definition

-- Drop table

-- DROP TABLE public.mileage_tracking;

CREATE TABLE public.mileage_tracking (
	id serial4 NOT NULL,
	date_of_trip date NOT NULL,
	trip_type varchar(50) NOT NULL,
	reason varchar(255) NULL,
	start_odometer numeric(10, 2) NULL,
	end_odometer numeric(10, 2) NULL,
	total_miles numeric(10, 2) NOT NULL,
	inventory_id int4 NULL,
	auction_site_id int4 NULL,
	irs_rate numeric(10, 4) NULL,
	created_at timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	updated_at timestamp NULL,
	CONSTRAINT mileage_tracking_pkey PRIMARY KEY (id)
);