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
	CONSTRAINT facebook_posts_pkey PRIMARY KEY (post_id)
);

-- Table Triggers

create trigger trg_facebook_posts_updated before
update
    on
    public.facebook_posts for each row execute function set_updated_at();


-- public.facebook_posts foreign keys

ALTER TABLE public.facebook_posts ADD CONSTRAINT fk_facebook_posts_acq FOREIGN KEY (acq_id) REFERENCES public.acquisitions(acq_id);