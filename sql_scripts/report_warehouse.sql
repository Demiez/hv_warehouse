SELECT DISTINCT
	p.part_id,
	p.part_name,
	(SELECT SUM(f.part_qty) FROM feeds f WHERE f.part_id = p.part_id) AS total_feeds,
	(SELECT SUM(s.shipment_qty) FROM shipments s WHERE s.part_id = p.part_id) AS total_shipments,
	w.part_qty AS warehouse_qty
FROM parts p LEFT JOIN feeds f ON p.part_id = f.part_id
LEFT JOIN shipments s ON p.part_id = s.part_id
JOIN warehouses w ON p.part_id = w.part_id;


SELECT DISTINCT
	p.part_id,
	p.part_name,
	feed_total(p.part_id),
	shipment_total(p.part_id),
	w.part_qty
FROM parts p LEFT JOIN feeds f ON p.part_id = f.part_id
LEFT JOIN warehouses w ON p.part_id = w.part_id
LEFT JOIN shipments s ON p.part_id = s.part_id;