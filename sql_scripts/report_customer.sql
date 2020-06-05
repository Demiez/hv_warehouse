SELECT
	c.customer_name,
	c.customer_address,
	p.part_name,
	s.shipment_qty,
	s.shipment_date
FROM shipments s JOIN customers c ON s.shipment_id = c.customer_id
JOIN parts p ON s.part_id = p.part_id;