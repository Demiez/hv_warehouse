INSERT INTO parts
VALUES
	('CAP-121200', 'capacitor', 'Panasonic', 79.99, '100 kV 1000pF'),
	('IND-040506', 'inductor', 'HALO Electronics', 9.50, '10 kV 120mH'),
	('RES-201744', 'resistor', 'KOA Europe GmbH', 22.43, '50 kV 1G'),
	('DIO-338801', 'diode', 'Abracon', 14.04, '2Cl 2Fp'),
	('TRA-182830', 'transformer', 'NEC TOKIN', 49.99, '15 kV AC_PC');
	
INSERT INTO warehouses
VALUES
	('WH-000001', 'CAP-121200', 10, '1006 Oaza Kadoma, Kadoma-shi, Osaka 571-8501', 'Japan'),
	('WH-000002', 'IND-040506', 10, '2933 Bunker Hill Lane, Suite 200 Santa Clara, CA', 'USA'),
	('WH-000003', 'RES-201744', 10, 'Kaddenbusch 6, 25578 Dägeling', 'Germany'),
	('WH-000004', 'DIO-338801', 10, '5101 Hidden Creek Ln, Spicewood, TX 78669', 'USA'),
	('WH-000005', 'TRA-182830', 10, '1 Ring Road, Barangay la Mesa, Calamba 4027, Luzon', 'Philippines');
	
INSERT INTO customers (customer_name, customer_address, customer_phone)
VALUES
	('Hallie Jackson', 'USA, 257, County Road 1950, 62835, Illinois', '(903) 497-4297'),
	('Ziggy Frame', 'USA, 236973, 190th Street, 57381, South Dakota', '(384) 520-6159'),
	('Crystal Blair', 'USA, 36, Darby Lane, 08330, New Jersey', '(624) 625-1564'),
	('Zain Bain', 'Germany, 180, Weinbergstraße, 93413, Bavaria', '(263) 617-9978'),
	('Diya Bowman', 'Germany, 2, Flöteweg, 32351, North Rhine-Westphalia', '(863) 279-3525');

INSERT INTO shipments (part_id, shipment_date, shipment_qty, customer_id)
VALUES
	('CAP-121200', NOW(), 3, 1),
	('RES-201744', NOW(), 2, 3),
	('DIO-338801', NOW(), 1, 4);
	

	
