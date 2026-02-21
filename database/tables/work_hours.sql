-- public.work_hours definition

-- Drop table

-- DROP TABLE public.work_hours;

CREATE TABLE public.work_hours (
	work_hours_id serial4 NOT NULL,
	work_category_id int4 NOT NULL,
	work_date date NOT NULL,
	start_time time NULL,
	end_time time NULL,
	hours_worked numeric(5, 2) NULL,
	miles_driven numeric(10, 2) NULL DEFAULT 0,
	notes varchar(255) NULL,
	created_at timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	modified_at timestamp NULL,
	CONSTRAINT work_hours_pkey PRIMARY KEY (work_hours_id)
);