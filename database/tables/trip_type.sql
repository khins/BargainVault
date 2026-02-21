-- public.trip_type definition

-- Drop table

-- DROP TABLE public.trip_type;

CREATE TABLE public.trip_type (
	trip_type_id serial4 NOT NULL,
	trip_type_name varchar(100) NOT NULL,
	CONSTRAINT trip_type_pkey PRIMARY KEY (trip_type_id),
	CONSTRAINT trip_type_trip_type_name_key UNIQUE (trip_type_name)
);