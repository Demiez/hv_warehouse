SELECT DISTINCT
	p.part_id,
	p.part_name,
	(SELECT SUM(s.shipment_qty) FROM shipments s WHERE s.part_id = p.part_id) AS total_shipments,
	shipment_date AS date
FROM parts p LEFT JOIN shipments s ON p.part_id = s.part_id
WHERE DATE_PART('month',shipment_date) = 5 AND DATE_PART('year',shipment_date) = 2020;