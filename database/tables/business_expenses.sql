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
	CONSTRAINT business_expenses_pkey PRIMARY KEY (expense_id)
);

-- Table Triggers

create trigger trg_business_expenses_updated before
update
    on
    public.business_expenses for each row execute function set_updated_at();


-- public.business_expenses foreign keys

ALTER TABLE public.business_expenses ADD CONSTRAINT fk_business_expenses_booth FOREIGN KEY (booth_id) REFERENCES public.booths(booth_id);