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
	CONSTRAINT inventory_locations_pkey PRIMARY KEY (inventory_location_id)
);

-- Table Triggers

create trigger trg_inventory_locations_updated before
update
    on
    public.inventory_locations for each row execute function update_timestamp();


-- public.inventory_locations foreign keys

ALTER TABLE public.inventory_locations ADD CONSTRAINT fk_inventory_locations_booth FOREIGN KEY (booth_id) REFERENCES public.booths(booth_id);
ALTER TABLE public.inventory_locations ADD CONSTRAINT fk_inventory_locations_item FOREIGN KEY (item_id) REFERENCES public.items(item_id);