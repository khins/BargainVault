-- DROP SCHEMA public;

CREATE SCHEMA public AUTHORIZATION pg_database_owner;

COMMENT ON SCHEMA public IS 'standard public schema';

-- DROP SEQUENCE public.acquisition_loss_loss_id_seq;

CREATE SEQUENCE public.acquisition_loss_loss_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.acquisitions_acq_id_seq;

CREATE SEQUENCE public.acquisitions_acq_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.auction_items_auction_item_id_seq;

CREATE SEQUENCE public.auction_items_auction_item_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.auction_sites_auction_site_id_seq;

CREATE SEQUENCE public.auction_sites_auction_site_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.booths_booth_id_seq;

CREATE SEQUENCE public.booths_booth_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.business_expenses_expense_id_seq;

CREATE SEQUENCE public.business_expenses_expense_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.data_entry_log_log_id_seq;

CREATE SEQUENCE public.data_entry_log_log_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.facebook_posts_post_id_seq;

CREATE SEQUENCE public.facebook_posts_post_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.inventory_locations_inventory_location_id_seq;

CREATE SEQUENCE public.inventory_locations_inventory_location_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.inventory_status_status_id_seq;

CREATE SEQUENCE public.inventory_status_status_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.items_item_id_seq;

CREATE SEQUENCE public.items_item_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.mileage_tracking_id_seq;

CREATE SEQUENCE public.mileage_tracking_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.retail_prices_retail_price_id_seq;

CREATE SEQUENCE public.retail_prices_retail_price_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.sales_channels_channel_id_seq;

CREATE SEQUENCE public.sales_channels_channel_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.sales_sale_id_seq;

CREATE SEQUENCE public.sales_sale_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.stores_store_id_seq;

CREATE SEQUENCE public.stores_store_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.trip_type_trip_type_id_seq;

CREATE SEQUENCE public.trip_type_trip_type_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.trip_types_id_seq;

CREATE SEQUENCE public.trip_types_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.work_category_work_category_id_seq;

CREATE SEQUENCE public.work_category_work_category_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.work_hours_work_hours_id_seq;

CREATE SEQUENCE public.work_hours_work_hours_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;-- public.auction_sites definition

-- Drop table

-- DROP TABLE public.auction_sites;

CREATE TABLE public.auction_sites (
	auction_site_id serial4 NOT NULL,
	"name" varchar(255) NOT NULL,
	address varchar(255) NULL,
	city varchar(100) NULL,
	tax_rate numeric(6, 4) NULL,
	buyer_premium numeric(12, 2) NULL,
	date_created timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	state varchar(2) NULL,
	CONSTRAINT auction_sites_pkey PRIMARY KEY (auction_site_id)
);


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


-- public.data_entry_log definition

-- Drop table

-- DROP TABLE public.data_entry_log;

CREATE TABLE public.data_entry_log (
	log_id bigserial NOT NULL,
	table_name varchar(100) NOT NULL,
	record_id int8 NOT NULL,
	action_type varchar(20) NOT NULL,
	entered_by varchar(100) NOT NULL,
	entry_timestamp timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
	change_details jsonb NULL,
	CONSTRAINT data_entry_log_action_type_check CHECK (((action_type)::text = ANY ((ARRAY['INSERT'::character varying, 'UPDATE'::character varying, 'DELETE'::character varying])::text[]))),
	CONSTRAINT data_entry_log_pkey PRIMARY KEY (log_id)
);
CREATE INDEX idx_log_table_record ON public.data_entry_log USING btree (table_name, record_id);


-- public.inventory_status definition

-- Drop table

-- DROP TABLE public.inventory_status;

CREATE TABLE public.inventory_status (
	status_id serial4 NOT NULL,
	status_name varchar(100) NOT NULL,
	CONSTRAINT inventory_status_pkey PRIMARY KEY (status_id)
);


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


-- public.sales_channels definition

-- Drop table

-- DROP TABLE public.sales_channels;

CREATE TABLE public.sales_channels (
	channel_id serial4 NOT NULL,
	channel_name varchar(50) NOT NULL,
	CONSTRAINT sales_channels_channel_name_key UNIQUE (channel_name),
	CONSTRAINT sales_channels_pkey PRIMARY KEY (channel_id)
);


-- public.stores definition

-- Drop table

-- DROP TABLE public.stores;

CREATE TABLE public.stores (
	store_id serial4 NOT NULL,
	store_name varchar(150) NOT NULL,
	store_type varchar(50) NULL,
	website_url varchar(255) NULL,
	created_at timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
	CONSTRAINT stores_pkey PRIMARY KEY (store_id),
	CONSTRAINT stores_store_name_uk UNIQUE (store_name)
);


-- public.trip_type definition

-- Drop table

-- DROP TABLE public.trip_type;

CREATE TABLE public.trip_type (
	trip_type_id serial4 NOT NULL,
	trip_type_name varchar(100) NOT NULL,
	CONSTRAINT trip_type_pkey PRIMARY KEY (trip_type_id),
	CONSTRAINT trip_type_trip_type_name_key UNIQUE (trip_type_name)
);


-- public.trip_types definition

-- Drop table

-- DROP TABLE public.trip_types;

CREATE TABLE public.trip_types (
	id serial4 NOT NULL,
	"name" varchar(100) NOT NULL,
	CONSTRAINT trip_types_name_key UNIQUE (name),
	CONSTRAINT trip_types_pkey PRIMARY KEY (id)
);


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


-- public.acquisitions definition

-- Drop table

-- DROP TABLE public.acquisitions;

CREATE TABLE public.acquisitions (
	acq_id serial4 NOT NULL,
	item_id int4 NOT NULL,
	source_type varchar(50) NULL,
	auction_site_id int4 NULL,
	date_acquired date NULL,
	qty_acquired int4 NULL DEFAULT 1,
	unit_hammer_price numeric(12, 2) NULL,
	buyer_premium numeric(12, 2) NULL,
	sales_tax_paid numeric(12, 2) NULL,
	total_settlement numeric(12, 2) NULL,
	status_id int4 NULL,
	tax_rate numeric(6, 4) NULL,
	sold bool NULL DEFAULT false,
	sale_id int4 NULL,
	personal bool NULL DEFAULT false,
	business_expense bool NULL DEFAULT false,
	CONSTRAINT acquisitions_pkey PRIMARY KEY (acq_id),
	CONSTRAINT fk_acq_item FOREIGN KEY (item_id) REFERENCES public.items(item_id),
	CONSTRAINT fk_acq_site FOREIGN KEY (auction_site_id) REFERENCES public.auction_sites(auction_site_id)
);


-- public.auction_items definition

-- Drop table

-- DROP TABLE public.auction_items;

CREATE TABLE public.auction_items (
	auction_item_id serial4 NOT NULL,
	item_name varchar(255) NOT NULL,
	description text NULL,
	created_at timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	auction_site_id int4 NULL,
	staged_for_auction bool NULL DEFAULT false,
	staged_date timestamp NULL,
	CONSTRAINT auction_items_pkey PRIMARY KEY (auction_item_id),
	CONSTRAINT fk_auctionitems_site FOREIGN KEY (auction_site_id) REFERENCES public.auction_sites(auction_site_id)
);


-- public.business_expenses definition

-- Drop table

-- DROP TABLE public.business_expenses;

CREATE TABLE public.business_expenses (
	expense_id serial4 NOT NULL,
	expense_date date NOT NULL,
	category varchar(100) NULL,
	vendor varchar(255) NULL,
	description varchar(255) NULL,
	amount numeric(12, 2) NOT NULL,
	payment_method varchar(50) NULL,
	booth_id int4 NULL,
	is_cogs bool NULL DEFAULT false,
	receipt_path varchar(500) NULL,
	created_at timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	updated_at timestamp NULL,
	CONSTRAINT business_expenses_pkey PRIMARY KEY (expense_id),
	CONSTRAINT fk_business_expenses_booth FOREIGN KEY (booth_id) REFERENCES public.booths(booth_id)
);

-- Table Triggers

create trigger trg_business_expenses_updated before
update
    on
    public.business_expenses for each row execute function set_updated_at();


-- public.facebook_posts definition

-- Drop table

-- DROP TABLE public.facebook_posts;

CREATE TABLE public.facebook_posts (
	post_id serial4 NOT NULL,
	acq_id int4 NOT NULL,
	post_date timestamp NULL,
	post_title varchar(255) NULL,
	post_description text NULL,
	asking_price numeric(12, 2) NULL,
	boosted bool NULL DEFAULT false,
	created_at timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	updated_at timestamp NULL,
	mark_as_sold bool NULL DEFAULT false,
	renew_date timestamp NULL,
	CONSTRAINT facebook_posts_pkey PRIMARY KEY (post_id),
	CONSTRAINT fk_facebook_posts_acq FOREIGN KEY (acq_id) REFERENCES public.acquisitions(acq_id)
);

-- Table Triggers

create trigger trg_facebook_posts_updated before
update
    on
    public.facebook_posts for each row execute function set_updated_at();


-- public.inventory_locations definition

-- Drop table

-- DROP TABLE public.inventory_locations;

CREATE TABLE public.inventory_locations (
	inventory_location_id serial4 NOT NULL,
	item_id int4 NOT NULL,
	booth_id int4 NULL,
	status_id int4 NULL,
	date_placed timestamp NULL,
	asking_price numeric(12, 2) NULL,
	notes text NULL,
	created_at timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	updated_at timestamp NULL,
	CONSTRAINT inventory_locations_pkey PRIMARY KEY (inventory_location_id),
	CONSTRAINT fk_inventory_locations_booth FOREIGN KEY (booth_id) REFERENCES public.booths(booth_id),
	CONSTRAINT fk_inventory_locations_item FOREIGN KEY (item_id) REFERENCES public.items(item_id)
);

-- Table Triggers

create trigger trg_inventory_locations_updated before
update
    on
    public.inventory_locations for each row execute function update_timestamp();


-- public.retail_prices definition

-- Drop table

-- DROP TABLE public.retail_prices;

CREATE TABLE public.retail_prices (
	retail_price_id serial4 NOT NULL,
	item_id int4 NOT NULL,
	retail_price numeric(12, 2) NULL,
	url_item_location text NULL,
	created_at timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	asin varchar(50) NULL,
	store_id int4 NULL,
	price_date timestamp NULL,
	is_sale_price bool NOT NULL DEFAULT false,
	notes text NULL,
	CONSTRAINT retail_prices_pkey PRIMARY KEY (retail_price_id),
	CONSTRAINT fk_retailprices_item FOREIGN KEY (item_id) REFERENCES public.items(item_id),
	CONSTRAINT retail_prices_store_fk FOREIGN KEY (store_id) REFERENCES public.stores(store_id) ON DELETE RESTRICT
);


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
	CONSTRAINT sales_pkey PRIMARY KEY (sale_id),
	CONSTRAINT fk_sales_booth FOREIGN KEY (booth_id) REFERENCES public.booths(booth_id),
	CONSTRAINT fk_sales_item FOREIGN KEY (item_id) REFERENCES public.items(item_id)
);


-- public.acquisition_loss definition

-- Drop table

-- DROP TABLE public.acquisition_loss;

CREATE TABLE public.acquisition_loss (
	loss_id serial4 NOT NULL,
	acq_id int4 NOT NULL,
	reason text NULL,
	created_at timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	"7" int4 NULL,
	"253" int4 NULL,
	"Missing parts - sending to donation" varchar(50) NULL,
	"12/4/2025 11:00:52" varchar(50) NULL,
	lossid int4 NULL,
	acqid int4 NULL,
	CONSTRAINT acquisition_loss_pkey PRIMARY KEY (loss_id),
	CONSTRAINT fk_acquisitionloss_acq FOREIGN KEY (acq_id) REFERENCES public.acquisitions(acq_id)
);


-- public.vw_auction_profit_analysis source

CREATE OR REPLACE VIEW public.vw_auction_profit_analysis
AS SELECT a.acq_id,
    i.title,
    i.lot_number,
    s.name AS auction_site,
    a.total_settlement,
    a.total_settlement * 1.55 AS suggested_booth_price,
    a.total_settlement * 1.55 - a.total_settlement AS potential_profit
   FROM acquisitions a
     JOIN items i ON i.item_id = a.item_id
     JOIN auction_sites s ON s.auction_site_id = a.auction_site_id
  WHERE i.title::text !~~* '%card%'::text AND a.personal = false AND a.business_expense = false;



CREATE OR REPLACE FUNCTION public.delete_acquisition(p_acq_id integer, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_old_row jsonb;
BEGIN
    SELECT to_jsonb(a)
    INTO v_old_row
    FROM public.acquisitions a
    WHERE a.acq_id = p_acq_id;

    IF v_old_row IS NULL THEN
        RAISE EXCEPTION 'Acquisition % not found', p_acq_id;
    END IF;

    DELETE FROM public.acquisitions
    WHERE acq_id = p_acq_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'acquisitions',
        p_acq_id,
        'DELETE',
        p_entered_by,
        v_old_row
    );
END;
$function$
;

CREATE OR REPLACE FUNCTION public.delete_facebook_post(p_post_id integer, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_acq_id   int;
    v_item_id  int;
    v_title    text;
BEGIN
    -- Capture related acquisition + item info BEFORE delete
    SELECT
        a.acq_id,
        i.item_id,
        i.title
    INTO
        v_acq_id,
        v_item_id,
        v_title
    FROM public.facebook_posts fp
    JOIN public.acquisitions a ON a.acq_id = fp.acq_id
    JOIN public.items i ON i.item_id = a.item_id
    WHERE fp.post_id = p_post_id;

   	IF v_acq_id IS NULL THEN
    	RAISE EXCEPTION 'Facebook post % not found', p_post_id;
	END IF;
   
    -- Delete facebook post
    DELETE FROM public.facebook_posts
    WHERE post_id = p_post_id;

    -- Log delete with full context
    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'facebook_posts',
        p_post_id,
        'DELETE',
        p_entered_by,
        jsonb_build_object(
            'post_id', p_post_id,
            'acq_id', v_acq_id,
            'item_id', v_item_id,
            'item_title', v_title
        )
    );

    RETURN p_post_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.delete_inventory_location(p_inventory_location_id integer, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
BEGIN
    DELETE FROM public.inventory_locations
    WHERE inventory_location_id = p_inventory_location_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by
    )
    VALUES (
        'inventory_locations',
        p_inventory_location_id,
        'DELETE',
        p_entered_by
    );

    RETURN p_inventory_location_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.delete_inventory_location_by_item(p_item_id integer, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_inventory_location_id int;
BEGIN
    SELECT inventory_location_id
    INTO v_inventory_location_id
    FROM inventory_locations
    WHERE item_id = p_item_id
    LIMIT 1;

    IF v_inventory_location_id IS NULL THEN
        RETURN;
    END IF;

    DELETE FROM inventory_locations
    WHERE inventory_location_id = v_inventory_location_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by
    )
    VALUES (
        'inventory_locations',
        v_inventory_location_id,
        'DELETE',
        p_entered_by
    );
END;
$function$
;

CREATE OR REPLACE FUNCTION public.delete_retail_price(p_retail_price_id integer, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_old jsonb;
BEGIN
    SELECT jsonb_build_object(
        'item_id', item_id,
        'store_id', store_id,
        'retail_price', retail_price,
        'price_date', price_date,
        'is_sale_price', is_sale_price,
        'notes', notes
    )
    INTO v_old
    FROM retail_prices
    WHERE retail_price_id = p_retail_price_id;

    DELETE FROM public.retail_prices
    WHERE retail_price_id = p_retail_price_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'retail_prices',
        p_retail_price_id,
        'DELETE',
        p_entered_by,
        v_old
    );
END;
$function$
;

CREATE OR REPLACE FUNCTION public.delete_sale(p_sale_id integer, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    DELETE FROM public.sales
    WHERE sale_id = p_sale_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by
    )
    VALUES (
        'sales',
        p_sale_id,
        'DELETE',
        p_entered_by
    );
END;
$function$
;

CREATE OR REPLACE FUNCTION public.global_search(p_search text)
 RETURNS TABLE(entity_type text, entity_id integer, display_text text, secondary_text text)
 LANGUAGE plpgsql
AS $function$
BEGIN
RETURN QUERY

-- Items
SELECT
    'Item'::text,
    i.item_id,
    i.title::text,
    NULL::text
FROM items i
WHERE i.title ILIKE '%' || p_search || '%'

UNION ALL

-- Acquisitions
SELECT
    'Acquisition'::text,
    a.acq_id,
    i.title::text,
    ('Acq #' || a.acq_id)::text
FROM acquisitions a
JOIN items i ON i.item_id = a.item_id
WHERE i.title ILIKE '%' || p_search || '%'

UNION ALL

-- Facebook Posts
SELECT
    'FacebookPost'::text,
    f.post_id,
    f.post_title::text,
    i.title::text
FROM facebook_posts f
JOIN acquisitions a ON a.acq_id = f.acq_id
JOIN items i ON i.item_id = a.item_id
WHERE f.post_title ILIKE '%' || p_search || '%';


END;
$function$
;

CREATE OR REPLACE FUNCTION public.insert_acquisition(p_item_id integer, p_source_type character varying, p_auction_site_id integer, p_date_acquired timestamp without time zone, p_qty_acquired integer, p_unit_hammer_price numeric, p_buyer_premium numeric, p_tax_rate numeric, p_sales_tax_paid numeric, p_total_settlement numeric, p_status_id integer, p_personal boolean, p_business_expense boolean, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_acq_id int;
    v_new_row jsonb;
BEGIN
    INSERT INTO public.acquisitions (
        item_id,
        source_type,
        auction_site_id,
        date_acquired,
        qty_acquired,
        unit_hammer_price,
        buyer_premium,
        tax_rate,
        sales_tax_paid,
        total_settlement,
        status_id,
        personal,
        business_expense
    )
    VALUES (
        p_item_id,
        p_source_type,
        p_auction_site_id,
        p_date_acquired ,
        COALESCE(p_qty_acquired, 1),
        p_unit_hammer_price,
        p_buyer_premium,
        p_tax_rate,
        p_sales_tax_paid,
        p_total_settlement,
        p_status_id,
        COALESCE(p_personal, false),
        COALESCE(p_business_expense, false)
    )
    RETURNING acq_id INTO v_acq_id;

    SELECT to_jsonb(a)
    INTO v_new_row
    FROM public.acquisitions a
    WHERE a.acq_id = v_acq_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'acquisitions',
        v_acq_id,
        'INSERT',
        p_entered_by,
        v_new_row
    );

    RETURN v_acq_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.insert_acquisition(p_item_id integer, p_source_type character varying, p_auction_site_id integer, p_date_acquired date, p_qty_acquired integer, p_unit_hammer_price numeric, p_buyer_premium numeric, p_tax_rate numeric, p_sales_tax_paid numeric, p_total_settlement numeric, p_status_id integer, p_personal boolean, p_business_expense boolean, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_acq_id int;
    v_new_row jsonb;
BEGIN
    INSERT INTO public.acquisitions (
        item_id,
        source_type,
        auction_site_id,
        date_acquired,
        qty_acquired,
        unit_hammer_price,
        buyer_premium,
        tax_rate,
        sales_tax_paid,
        total_settlement,
        status_id,
        personal,
        business_expense
    )
    VALUES (
        p_item_id,
        p_source_type,
        p_auction_site_id,
        p_date_acquired,
        COALESCE(p_qty_acquired, 1),
        p_unit_hammer_price,
        p_buyer_premium,
        p_tax_rate,
        p_sales_tax_paid,
        p_total_settlement,
        p_status_id,
        COALESCE(p_personal, false),
        COALESCE(p_business_expense, false)
    )
    RETURNING acq_id INTO v_acq_id;

    SELECT to_jsonb(a)
    INTO v_new_row
    FROM public.acquisitions a
    WHERE a.acq_id = v_acq_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'acquisitions',
        v_acq_id,
        'INSERT',
        p_entered_by,
        v_new_row
    );

    RETURN v_acq_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.insert_facebook_post(p_acq_id integer, p_post_date timestamp without time zone, p_post_title character varying, p_post_description text, p_asking_price numeric, p_boosted boolean, p_mark_as_sold boolean, p_renew_date timestamp without time zone, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_post_id int;
BEGIN
    INSERT INTO public.facebook_posts (
        acq_id,
        post_date,
        post_title,
        post_description,
        asking_price,
        boosted,
        mark_as_sold,
        renew_date
    )
    VALUES (
        p_acq_id,
        p_post_date,
        p_post_title,
        p_post_description,
        p_asking_price,
        COALESCE(p_boosted, false),
        COALESCE(p_mark_as_sold, false),
        p_renew_date
    )
    RETURNING post_id INTO v_post_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'facebook_posts',
        v_post_id,
        'INSERT',
        p_entered_by,
        jsonb_build_object(
            'acq_id', p_acq_id,
            'asking_price', p_asking_price,
            'boosted', p_boosted
        )
    );

    RETURN v_post_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.insert_facebook_post(p_acq_id integer, p_post_date timestamp with time zone, p_post_title character varying, p_post_description text, p_asking_price numeric, p_boosted boolean, p_mark_as_sold boolean, p_renew_date timestamp without time zone, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_post_id int;
BEGIN
    INSERT INTO public.facebook_posts (
        acq_id,
        post_date,
        post_title,
        post_description,
        asking_price,
        boosted,
        mark_as_sold,
        renew_date
    )
    VALUES (
        p_acq_id,
        p_post_date,
        p_post_title,
        p_post_description,
        p_asking_price,
        COALESCE(p_boosted, false),
        COALESCE(p_mark_as_sold, false),
        p_renew_date
    )
    RETURNING post_id INTO v_post_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'facebook_posts',
        v_post_id,
        'INSERT',
        p_entered_by,
        jsonb_build_object(
            'acq_id', p_acq_id,
            'asking_price', p_asking_price,
            'boosted', p_boosted
        )
    );

    RETURN v_post_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.insert_inventory_location(p_item_id integer, p_booth_id integer, p_status_id integer, p_date_placed timestamp with time zone, p_asking_price numeric, p_notes text, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_inventory_location_id int;
BEGIN
    INSERT INTO public.inventory_locations (
        item_id,
        booth_id,
        status_id,
        date_placed,
        asking_price,
        notes
    )
    VALUES (
        p_item_id,
        p_booth_id,
        p_status_id,
        p_date_placed,
        p_asking_price,
        p_notes
    )
    RETURNING inventory_location_id
    INTO v_inventory_location_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'inventory_locations',
        v_inventory_location_id,
        'INSERT',
        p_entered_by,
        jsonb_build_object(
            'item_id', p_item_id,
            'booth_id', p_booth_id,
            'status_id', p_status_id,
            'date_placed', p_date_placed,
            'asking_price', p_asking_price
        )
    );

    RETURN v_inventory_location_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.insert_inventory_location(p_item_id integer, p_booth_id integer, p_status_id integer, p_date_placed timestamp without time zone, p_asking_price numeric, p_notes text, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_inventory_location_id int;
BEGIN
    INSERT INTO public.inventory_locations (
        item_id,
        booth_id,
        status_id,
        date_placed,
        asking_price,
        notes
    )
    VALUES (
        p_item_id,
        p_booth_id,
        p_status_id,
        p_date_placed,
        p_asking_price,
        p_notes
    )
    RETURNING inventory_location_id
    INTO v_inventory_location_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'inventory_locations',
        v_inventory_location_id,
        'INSERT',
        p_entered_by,
        jsonb_build_object(
            'item_id', p_item_id,
            'booth_id', p_booth_id,
            'status_id', p_status_id,
            'date_placed', p_date_placed,
            'asking_price', p_asking_price
        )
    );

    RETURN v_inventory_location_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.insert_item(p_lot_number integer, p_title character varying, p_description text, p_image_path character varying, p_quantity integer, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_item_id INT;
BEGIN
    -- Basic validation
    IF p_title IS NULL OR length(trim(p_title)) = 0 THEN
        RAISE EXCEPTION 'Title is required';
    END IF;

    -- Insert item
    INSERT INTO public.items (
        lot_number,
        title,
        description,
        image_path,
        quantity
    )
    VALUES (
        p_lot_number,
        p_title,
        p_description,
        p_image_path,
        COALESCE(p_quantity, 1)
    )
    RETURNING item_id INTO v_item_id;

    -- Log entry
    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'items',
        v_item_id,
        'INSERT',
        p_entered_by,
        jsonb_build_object(
            'lot_number', p_lot_number,
            'title', p_title,
            'description', p_description,
            'image_path', p_image_path,
            'quantity', COALESCE(p_quantity, 1)
        )
    );

    RETURN v_item_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.insert_retail_price(p_item_id integer, p_store_id integer, p_retail_price numeric, p_price_date timestamp with time zone, p_is_sale_price boolean, p_notes text, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_retail_price_id int;
BEGIN
    INSERT INTO public.retail_prices (
        item_id,
        store_id,
        retail_price,
        price_date,
        is_sale_price,
        notes
    )
    VALUES (
        p_item_id,
        p_store_id,
        p_retail_price,
        p_price_date,
        COALESCE(p_is_sale_price, false),
        p_notes
    )
    RETURNING retail_price_id
    INTO v_retail_price_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'retail_prices',
        v_retail_price_id,
        'INSERT',
        p_entered_by,
        jsonb_build_object(
            'item_id', p_item_id,
            'store_id', p_store_id,
            'retail_price', p_retail_price,
            'price_date', p_price_date,
            'is_sale_price', p_is_sale_price,
            'notes', p_notes
        )
    );

    RETURN v_retail_price_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.insert_retail_price(p_item_id integer, p_store_id integer, p_price numeric, p_price_date date, p_is_sale boolean, p_notes text, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_new_id int;
BEGIN
    INSERT INTO retail_prices (
        item_id,
        store_id,
        retail_price,
        price_date,
        is_sale_price,
        notes
    )
    VALUES (
        p_item_id,
        p_store_id,
        p_price,
        p_price_date,
        p_is_sale,
        p_notes
    )
    RETURNING retail_price_id
    INTO v_new_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'retail_prices',
        v_new_id,
        'INSERT',
        p_entered_by,
        jsonb_build_object(
            'item_id', p_item_id,
            'store_id', p_store_id,
            'price', p_price,
            'price_date', p_price_date
        )
    );

    RETURN v_new_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.insert_retail_price(p_item_id integer, p_store_id integer, p_retail_price numeric, p_price_date timestamp without time zone, p_is_sale_price boolean, p_notes text, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_retail_price_id int;
BEGIN
    INSERT INTO public.retail_prices (
        item_id,
        store_id,
        retail_price,
        price_date,
        is_sale_price,
        notes
    )
    VALUES (
        p_item_id,
        p_store_id,
        p_retail_price,
        p_price_date,
        COALESCE(p_is_sale_price, false),
        p_notes
    )
    RETURNING retail_price_id
    INTO v_retail_price_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'retail_prices',
        v_retail_price_id,
        'INSERT',
        p_entered_by,
        jsonb_build_object(
            'item_id', p_item_id,
            'store_id', p_store_id,
            'retail_price', p_retail_price,
            'price_date', p_price_date,
            'is_sale_price', p_is_sale_price,
            'notes', p_notes
        )
    );

    RETURN v_retail_price_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.insert_sale(p_item_id integer, p_date_sold timestamp with time zone, p_qty_sold integer, p_channel_type character varying, p_booth_id integer, p_unit_sale_price numeric, p_discounted_rate numeric, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_sale_id integer;
BEGIN
    INSERT INTO public.sales (
        item_id,
        date_sold,
        qty_sold,
        channel_type,
        booth_id,
        unit_sale_price,
        discounted_rate
    )
    VALUES (
        p_item_id,
        p_date_sold,
        COALESCE(p_qty_sold, 1),
        p_channel_type,
        p_booth_id,
        p_unit_sale_price,
        p_discounted_rate
    )
    RETURNING sale_id INTO v_sale_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'sales',
        v_sale_id,
        'INSERT',
        p_entered_by,
        jsonb_build_object(
            'item_id', p_item_id,
            'qty_sold', p_qty_sold,
            'unit_sale_price', p_unit_sale_price
        )
    );

    RETURN v_sale_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.insert_sale(p_item_id integer, p_date_sold timestamp without time zone, p_qty_sold integer, p_channel_type character varying, p_booth_id integer, p_unit_sale_price numeric, p_discounted_rate numeric, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_sale_id integer;
BEGIN
    INSERT INTO public.sales (
        item_id,
        date_sold,
        qty_sold,
        channel_type,
        booth_id,
        unit_sale_price,
        discounted_rate
    )
    VALUES (
        p_item_id,
        p_date_sold,
        COALESCE(p_qty_sold, 1),
        p_channel_type,
        p_booth_id,
        p_unit_sale_price,
        p_discounted_rate
    )
    RETURNING sale_id INTO v_sale_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'sales',
        v_sale_id,
        'INSERT',
        p_entered_by,
        jsonb_build_object(
            'item_id', p_item_id,
            'qty_sold', p_qty_sold,
            'unit_sale_price', p_unit_sale_price
        )
    );

    RETURN v_sale_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.mark_facebook_posts_sold_by_acq(p_acq_id integer, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_rows INT;
BEGIN
    UPDATE facebook_posts
    SET
        mark_as_sold = TRUE,
        updated_at = CURRENT_TIMESTAMP
    WHERE acq_id = p_acq_id
      AND mark_as_sold = FALSE;

    GET DIAGNOSTICS v_rows = ROW_COUNT;

    IF v_rows > 0 THEN
        INSERT INTO data_entry_log (
            table_name,
            record_id,
            action_type,
            entered_by,
            change_details
        )
        VALUES (
            'facebook_posts',
            p_acq_id,
            'UPDATE',
            p_entered_by,
            jsonb_build_object(
                'action', 'auto_mark_sold_from_sale',
                'acq_id', p_acq_id,
                'rows_updated', v_rows
            )
        );
    END IF;

    RETURN v_rows;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.mark_facebook_posts_sold_by_item(p_item_id integer, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_rows INT;
BEGIN
    UPDATE facebook_posts fp
    SET
        mark_as_sold = TRUE,
        updated_at = CURRENT_TIMESTAMP
    FROM acquisitions a
    WHERE fp.acq_id = a.acq_id
      AND a.item_id = p_item_id
      AND fp.mark_as_sold = FALSE;

    GET DIAGNOSTICS v_rows = ROW_COUNT;

    IF v_rows > 0 THEN
        INSERT INTO data_entry_log (
            table_name,
            record_id,
            action_type,
            entered_by,
            change_details
        )
        VALUES (
            'facebook_posts',
            p_item_id,
            'UPDATE',
            p_entered_by,
            jsonb_build_object(
                'action', 'auto_mark_sold_from_sale',
                'item_id', p_item_id,
                'rows_updated', v_rows
            )
        );
    END IF;

    RETURN v_rows;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.set_updated_at()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_acquisition(p_acq_id integer, p_item_id integer, p_source_type character varying, p_auction_site_id integer, p_date_acquired date, p_qty_acquired integer, p_unit_hammer_price numeric, p_buyer_premium numeric, p_tax_rate numeric, p_sales_tax_paid numeric, p_total_settlement numeric, p_status_id integer, p_personal boolean, p_business_expense boolean, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_old_row jsonb;
    v_new_row jsonb;
BEGIN
    SELECT to_jsonb(a)
    INTO v_old_row
    FROM public.acquisitions a
    WHERE a.acq_id = p_acq_id;

    IF v_old_row IS NULL THEN
        RAISE EXCEPTION 'Acquisition % not found', p_acq_id;
    END IF;

    UPDATE public.acquisitions
    SET
        item_id = p_item_id,
        source_type = p_source_type,
        auction_site_id = p_auction_site_id,
        date_acquired = p_date_acquired,
        qty_acquired = COALESCE(p_qty_acquired, 1),
        unit_hammer_price = p_unit_hammer_price,
        buyer_premium = p_buyer_premium,
        tax_rate = p_tax_rate,
        sales_tax_paid = p_sales_tax_paid,
        total_settlement = p_total_settlement,
        status_id = p_status_id,
        personal = COALESCE(p_personal, false),
        business_expense = COALESCE(p_business_expense, false)
    WHERE acq_id = p_acq_id;

    SELECT to_jsonb(a)
    INTO v_new_row
    FROM public.acquisitions a
    WHERE a.acq_id = p_acq_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'acquisitions',
        p_acq_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'before', v_old_row,
            'after', v_new_row
        )
    );
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_acquisition(p_acq_id integer, p_item_id integer, p_source_type text, p_auction_site_id integer, p_date_acquired timestamp without time zone, p_qty_acquired integer, p_unit_hammer_price numeric, p_buyer_premium numeric, p_tax_rate numeric, p_sales_tax_paid numeric, p_total_settlement numeric, p_status_id integer, p_personal boolean, p_business_expense boolean, p_entered_by text)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE acquisitions
    SET
        item_id            = p_item_id,
        source_type         = p_source_type,
        auction_site_id     = p_auction_site_id,
        date_acquired       = p_date_acquired,
        qty_acquired        = p_qty_acquired,
        unit_hammer_price   = p_unit_hammer_price,
        buyer_premium       = p_buyer_premium,
        tax_rate            = p_tax_rate,
        sales_tax_paid      = p_sales_tax_paid,
        total_settlement    = p_total_settlement,
        status_id           = p_status_id,
        personal            = p_personal,
        business_expense    = p_business_expense
    WHERE acq_id = p_acq_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'acquisitions',
        p_acq_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'item_id', p_item_id,
            'total_settlement', p_total_settlement
        )
    );
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_facebook_post(p_post_id integer, p_post_date timestamp without time zone, p_post_title character varying, p_post_description text, p_asking_price numeric, p_boosted boolean, p_mark_as_sold boolean, p_renew_date timestamp without time zone, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE public.facebook_posts
    SET
        post_date = p_post_date,
        post_title = p_post_title,
        post_description = p_post_description,
        asking_price = p_asking_price,
        boosted = COALESCE(p_boosted, boosted),
        mark_as_sold = COALESCE(p_mark_as_sold, mark_as_sold),
        renew_date = p_renew_date,
        updated_at = CURRENT_TIMESTAMP
    WHERE post_id = p_post_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'facebook_posts',
        p_post_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'asking_price', p_asking_price,
            'boosted', p_boosted,
            'mark_as_sold', p_mark_as_sold
        )
    );

    RETURN p_post_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_inventory_location(p_inventory_location_id integer, p_item_id integer, p_booth_id integer, p_status_id integer, p_date_placed timestamp without time zone, p_asking_price numeric, p_notes text, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE public.inventory_locations
    SET
        item_id = p_item_id,
        booth_id = p_booth_id,
        status_id = p_status_id,
        date_placed = p_date_placed,
        asking_price = p_asking_price,
        notes = p_notes,
        updated_at = CURRENT_TIMESTAMP
    WHERE inventory_location_id = p_inventory_location_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'inventory_locations',
        p_inventory_location_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'item_id', p_item_id,
            'booth_id', p_booth_id,
            'status_id', p_status_id,
            'date_placed', p_date_placed,
            'asking_price', p_asking_price
        )
    );

    RETURN p_inventory_location_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_inventory_location(p_inventory_location_id integer, p_item_id integer, p_booth_id integer, p_status_id integer, p_date_placed timestamp with time zone, p_asking_price numeric, p_notes text, p_entered_by character varying)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE public.inventory_locations
    SET
        item_id = p_item_id,
        booth_id = p_booth_id,
        status_id = p_status_id,
        date_placed = p_date_placed,
        asking_price = p_asking_price,
        notes = p_notes,
        updated_at = CURRENT_TIMESTAMP
    WHERE inventory_location_id = p_inventory_location_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'inventory_locations',
        p_inventory_location_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'item_id', p_item_id,
            'booth_id', p_booth_id,
            'status_id', p_status_id,
            'date_placed', p_date_placed,
            'asking_price', p_asking_price
        )
    );

    RETURN p_inventory_location_id;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_item(p_item_id integer, p_lot_number integer, p_title character varying, p_description text, p_image_path character varying, p_quantity integer, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_old RECORD;
BEGIN
    SELECT *
    INTO v_old
    FROM public.items
    WHERE item_id = p_item_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'Item % not found', p_item_id;
    END IF;

    UPDATE public.items
    SET
        lot_number  = p_lot_number,
        title       = p_title,
        description = p_description,
        image_path  = p_image_path,
        quantity    = p_quantity
    WHERE item_id = p_item_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'items',
        p_item_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'old', row_to_json(v_old),
            'new', jsonb_build_object(
                'lot_number', p_lot_number,
                'title', p_title,
                'description', p_description,
                'image_path', p_image_path,
                'quantity', p_quantity
            )
        )
    );
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_retail_price(p_retail_price_id integer, p_item_id integer, p_store_id integer, p_retail_price numeric, p_price_date timestamp with time zone, p_is_sale_price boolean, p_notes text, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_old jsonb;
BEGIN
    SELECT jsonb_build_object(
        'item_id', item_id,
        'store_id', store_id,
        'retail_price', retail_price,
        'price_date', price_date,
        'is_sale_price', is_sale_price,
        'notes', notes
    )
    INTO v_old
    FROM retail_prices
    WHERE retail_price_id = p_retail_price_id;

    UPDATE public.retail_prices
    SET
        item_id = p_item_id,
        store_id = p_store_id,
        retail_price = p_retail_price,
        price_date = p_price_date,
        is_sale_price = COALESCE(p_is_sale_price, false),
        notes = p_notes
    WHERE retail_price_id = p_retail_price_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'retail_prices',
        p_retail_price_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'before', v_old,
            'after', jsonb_build_object(
                'item_id', p_item_id,
                'store_id', p_store_id,
                'retail_price', p_retail_price,
                'price_date', p_price_date,
                'is_sale_price', p_is_sale_price,
                'notes', p_notes
            )
        )
    );
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_retail_price(p_retail_price_id integer, p_item_id integer, p_store_id integer, p_retail_price numeric, p_price_date timestamp without time zone, p_is_sale_price boolean, p_notes text, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
    v_old jsonb;
BEGIN
    SELECT jsonb_build_object(
        'item_id', item_id,
        'store_id', store_id,
        'retail_price', retail_price,
        'price_date', price_date,
        'is_sale_price', is_sale_price,
        'notes', notes
    )
    INTO v_old
    FROM retail_prices
    WHERE retail_price_id = p_retail_price_id;

    UPDATE public.retail_prices
    SET
        item_id = p_item_id,
        store_id = p_store_id,
        retail_price = p_retail_price,
        price_date = p_price_date,
        is_sale_price = COALESCE(p_is_sale_price, false),
        notes = p_notes,
        updated_at = CURRENT_TIMESTAMP
    WHERE retail_price_id = p_retail_price_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'retail_prices',
        p_retail_price_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'before', v_old,
            'after', jsonb_build_object(
                'item_id', p_item_id,
                'store_id', p_store_id,
                'retail_price', p_retail_price,
                'price_date', p_price_date,
                'is_sale_price', p_is_sale_price,
                'notes', p_notes
            )
        )
    );
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_sale(p_sale_id integer, p_item_id integer, p_date_sold timestamp with time zone, p_qty_sold integer, p_channel_type character varying, p_booth_id integer, p_unit_sale_price numeric, p_discounted_rate numeric, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE public.sales
    SET
        item_id           = p_item_id,
        date_sold         = p_date_sold,
        qty_sold          = p_qty_sold,
        channel_type      = p_channel_type,
        booth_id          = p_booth_id,
        unit_sale_price   = p_unit_sale_price,
        discounted_rate   = p_discounted_rate
    WHERE sale_id = p_sale_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'sales',
        p_sale_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'qty_sold', p_qty_sold,
            'unit_sale_price', p_unit_sale_price,
            'discounted_rate', p_discounted_rate
        )
    );
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_sale(p_sale_id integer, p_item_id integer, p_date_sold timestamp without time zone, p_qty_sold integer, p_channel_type character varying, p_booth_id integer, p_unit_sale_price numeric, p_discounted_rate numeric, p_entered_by character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE public.sales
    SET
        item_id           = p_item_id,
        date_sold         = p_date_sold,
        qty_sold          = p_qty_sold,
        channel_type      = p_channel_type,
        booth_id          = p_booth_id,
        unit_sale_price   = p_unit_sale_price,
        discounted_rate   = p_discounted_rate
    WHERE sale_id = p_sale_id;

    INSERT INTO public.data_entry_log (
        table_name,
        record_id,
        action_type,
        entered_by,
        change_details
    )
    VALUES (
        'sales',
        p_sale_id,
        'UPDATE',
        p_entered_by,
        jsonb_build_object(
            'qty_sold', p_qty_sold,
            'unit_sale_price', p_unit_sale_price,
            'discounted_rate', p_discounted_rate
        )
    );
END;
$function$
;

CREATE OR REPLACE FUNCTION public.update_timestamp()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$function$
;
