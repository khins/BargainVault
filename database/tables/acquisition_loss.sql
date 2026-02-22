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
	CONSTRAINT acquisition_loss_pkey PRIMARY KEY (loss_id)
);


-- public.acquisition_loss foreign keys

ALTER TABLE public.acquisition_loss ADD CONSTRAINT fk_acquisitionloss_acq FOREIGN KEY (acq_id) REFERENCES public.acquisitions(acq_id);