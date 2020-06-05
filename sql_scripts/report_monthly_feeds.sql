SELECT DISTINCT
	p.part_id,
	p.part_name,
	(SELECT SUM(f.part_qty) FROM feeds f WHERE f.part_id = p.part_id) AS total_feeds,
	feed_date AS date
FROM parts p LEFT JOIN feeds f ON p.part_id = f.part_id
LEFT JOIN shipments s ON p.part_id = s.part_id
JOIN warehouses w ON p.part_id = w.part_id
WHERE DATE_PART('month',feed_date) = 5 AND DATE_PART('year',feed_date) = 2020;