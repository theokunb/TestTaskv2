--
-- PostgreSQL database dump
--

-- Dumped from database version 15.3 (Debian 15.3-1.pgdg120+1)
-- Dumped by pg_dump version 15.3

-- Started on 2024-11-14 15:35:50

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 4 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: pg_database_owner
--

CREATE SCHEMA public;


ALTER SCHEMA public OWNER TO pg_database_owner;

--
-- TOC entry 3373 (class 0 OID 0)
-- Dependencies: 4
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: pg_database_owner
--

COMMENT ON SCHEMA public IS 'standard public schema';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 215 (class 1259 OID 16509)
-- Name: customer; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.customer (
    id integer NOT NULL,
    fullname character varying,
    inn character varying
);


ALTER TABLE public.customer OWNER TO postgres;

--
-- TOC entry 214 (class 1259 OID 16508)
-- Name: customer_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.customer_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.customer_id_seq OWNER TO postgres;

--
-- TOC entry 3374 (class 0 OID 0)
-- Dependencies: 214
-- Name: customer_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.customer_id_seq OWNED BY public.customer.id;


--
-- TOC entry 219 (class 1259 OID 16527)
-- Name: customerpurchase; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.customerpurchase (
    id integer NOT NULL,
    purchaseid integer,
    customerid integer
);


ALTER TABLE public.customerpurchase OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 16526)
-- Name: customerpurchase_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.customerpurchase_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.customerpurchase_id_seq OWNER TO postgres;

--
-- TOC entry 3375 (class 0 OID 0)
-- Dependencies: 218
-- Name: customerpurchase_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.customerpurchase_id_seq OWNED BY public.customerpurchase.id;


--
-- TOC entry 217 (class 1259 OID 16518)
-- Name: purchasedata; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.purchasedata (
    id integer NOT NULL,
    purchasenumber character varying,
    purchaseobjectinfo character varying,
    price real,
    docpublishdate date
);


ALTER TABLE public.purchasedata OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 16517)
-- Name: purchasedata_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.purchasedata_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.purchasedata_id_seq OWNER TO postgres;

--
-- TOC entry 3376 (class 0 OID 0)
-- Dependencies: 216
-- Name: purchasedata_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.purchasedata_id_seq OWNED BY public.purchasedata.id;


--
-- TOC entry 3209 (class 2604 OID 16512)
-- Name: customer id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customer ALTER COLUMN id SET DEFAULT nextval('public.customer_id_seq'::regclass);


--
-- TOC entry 3211 (class 2604 OID 16530)
-- Name: customerpurchase id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customerpurchase ALTER COLUMN id SET DEFAULT nextval('public.customerpurchase_id_seq'::regclass);


--
-- TOC entry 3210 (class 2604 OID 16521)
-- Name: purchasedata id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.purchasedata ALTER COLUMN id SET DEFAULT nextval('public.purchasedata_id_seq'::regclass);


--
-- TOC entry 3363 (class 0 OID 16509)
-- Dependencies: 215
-- Data for Name: customer; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3367 (class 0 OID 16527)
-- Dependencies: 219
-- Data for Name: customerpurchase; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3365 (class 0 OID 16518)
-- Dependencies: 217
-- Data for Name: purchasedata; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3377 (class 0 OID 0)
-- Dependencies: 214
-- Name: customer_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.customer_id_seq', 1, false);


--
-- TOC entry 3378 (class 0 OID 0)
-- Dependencies: 218
-- Name: customerpurchase_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.customerpurchase_id_seq', 1, false);


--
-- TOC entry 3379 (class 0 OID 0)
-- Dependencies: 216
-- Name: purchasedata_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.purchasedata_id_seq', 1, false);


--
-- TOC entry 3213 (class 2606 OID 16516)
-- Name: customer customer_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customer
    ADD CONSTRAINT customer_pk PRIMARY KEY (id);


--
-- TOC entry 3217 (class 2606 OID 16532)
-- Name: customerpurchase customerpurchase_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customerpurchase
    ADD CONSTRAINT customerpurchase_pk PRIMARY KEY (id);


--
-- TOC entry 3215 (class 2606 OID 16525)
-- Name: purchasedata purchasedata_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.purchasedata
    ADD CONSTRAINT purchasedata_pk PRIMARY KEY (id);


--
-- TOC entry 3218 (class 2606 OID 16533)
-- Name: customerpurchase customerpurchase_customer_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customerpurchase
    ADD CONSTRAINT customerpurchase_customer_fk FOREIGN KEY (customerid) REFERENCES public.customer(id);


--
-- TOC entry 3219 (class 2606 OID 16538)
-- Name: customerpurchase customerpurchase_purchasedata_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customerpurchase
    ADD CONSTRAINT customerpurchase_purchasedata_fk FOREIGN KEY (purchaseid) REFERENCES public.purchasedata(id);


-- Completed on 2024-11-14 15:35:50

--
-- PostgreSQL database dump complete
--

