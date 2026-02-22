-- public.work_category definition

-- Drop table

-- DROP TABLE public.work_category;

CREATE TABLE public.work_category (
	work_category_id serial4 NOT NULL,
	category_name varchar(100) NOT NULL,
	category_description varchar(255) NULL,
	CONSTRAINT work_category_category_name_key UNIQUE (category_name),
	CONSTRAINT work_category_pkey PRIMARY KEY (work_category_id)
);