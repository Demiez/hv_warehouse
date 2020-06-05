-- CREATE DATABASE warehouse_db;

CREATE TABLE parts (
	part_id varchar(10),
	part_name varchar(20) NOT NULL,
	part_mfr varchar(50),
	part_price numeric(10,2),
	part_desc varchar(200),
	CONSTRAINT part_key PRIMARY KEY (part_id)
);

CREATE TABLE warehouses (
	warehouse_id varchar(10),
	part_id varchar(10) REFERENCES parts(part_id),
	part_qty int,
	warehouse_address varchar(100) NOT NULL,
	warehouse_location varchar(50) NOT NULL,
	CONSTRAINT warehouse_key PRIMARY KEY (warehouse_id)
);

CREATE TABLE feeds (
	feed_id serial,
	part_id varchar(10) REFERENCES parts(part_id),
	part_qty int,
	feed_date date NOT NULL,
	CONSTRAINT feed_key PRIMARY KEY (feed_id),
	CONSTRAINT check_feed_qty_not_zero CHECK (part_qty > 0)
);

CREATE TABLE customers (
	customer_id serial,
	customer_name varchar(50),
	customer_address varchar(200),
	customer_phone varchar(30),
	CONSTRAINT customer_key PRIMARY KEY (customer_id)
);

CREATE TABLE shipments (
	shipment_id serial,
	part_id varchar(10) REFERENCES parts(part_id),
	shipment_date date NOT NULL,
	shipment_qty int,
	customer_id int REFERENCES customers(customer_id),
	CONSTRAINT shipment_key PRIMARY KEY (shipment_id)
);