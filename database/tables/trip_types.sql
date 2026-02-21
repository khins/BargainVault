-- public.trip_types definition

-- Drop table

-- DROP TABLE public.trip_types;

CREATE TABLE public.trip_types (
	id serial4 NOT NULL,
	"name" varchar(100) NOT NULL,
	CONSTRAINT trip_types_name_key UNIQUE (name),
	CONSTRAINT trip_types_pkey PRIMARY KEY (id)
);