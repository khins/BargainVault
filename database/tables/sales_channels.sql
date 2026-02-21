-- public.sales_channels definition

-- Drop table

-- DROP TABLE public.sales_channels;

CREATE TABLE public.sales_channels (
	channel_id serial4 NOT NULL,
	channel_name varchar(50) NOT NULL,
	CONSTRAINT sales_channels_channel_name_key UNIQUE (channel_name),
	CONSTRAINT sales_channels_pkey PRIMARY KEY (channel_id)
);