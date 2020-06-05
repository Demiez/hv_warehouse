CREATE VIEW production_modification
AS
SELECT DISTINCT
	p.part_id,
	p.part_name,
	(SELECT SUM(s.shipment_qty) FROM shipments s WHERE s.part_id = p.part_id) AS total_shipments,
	(SELECT CAST(AVG(s.shipment_qty) AS numeric(10,2)) FROM shipments s WHERE s.part_id = p.part_id) AS average_shipments,
	(SELECT CAST(AVG(s.shipment_qty) AS numeric(10,2)) FROM shipments s WHERE s.part_id = p.part_id) * 6 AS average_required,
	(w.part_qty - (SELECT CAST(AVG(s.shipment_qty) AS numeric(10,2)) FROM shipments s WHERE s.part_id = p.part_id) * 6) * -1 AS modify_production
FROM parts p LEFT JOIN shipments s ON p.part_id = s.part_id
JOIN warehouses w ON p.part_id = w.part_id
WHERE s.shipment_date > date_trunc('day', NOW() - interval '6 month');