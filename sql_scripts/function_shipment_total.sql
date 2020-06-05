CREATE OR REPLACE FUNCTION shipment_total (_id varchar(10))
RETURNS integer AS $total$
declare
	total integer;
BEGIN
   SELECT SUM (shipment_qty) into total
   FROM shipments
   WHERE part_id = _id;
   RETURN total;
END;
$total$ LANGUAGE plpgsql;