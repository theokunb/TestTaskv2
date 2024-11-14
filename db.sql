CREATE TABLE public.customer (
	id serial NOT NULL,
	fullname varchar NULL,
	inn varchar NULL,
	CONSTRAINT customer_pk PRIMARY KEY (id)
);


CREATE TABLE public.purchasedata (
	id serial NOT NULL,
	purchasenumber varchar NULL,
	purchaseobjectinfo varchar NULL,
	docpublishdate date NULL,
	CONSTRAINT purchasedata_pk PRIMARY KEY (id)
);

CREATE TABLE public.customerpurchase (
	id serial NOT NULL,
	purchaseid int NULL,
	customerid int NULL,
	CONSTRAINT customerpurchase_pk PRIMARY KEY (id),
	CONSTRAINT customerpurchase_customer_fk FOREIGN KEY (customerid) REFERENCES public.customer(id),
	CONSTRAINT customerpurchase_purchasedata_fk FOREIGN KEY (purchaseid) REFERENCES public.purchasedata(id)
);
