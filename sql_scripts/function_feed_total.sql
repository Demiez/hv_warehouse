CREATE OR REPLACE FUNCTION feed_total (_id varchar(10))
RETURNS integer AS $total$
declare
	total integer;
BEGIN
   SELECT SUM (part_qty) into total
   FROM feeds
   WHERE part_id = _id;
   RETURN total;
END;
$total$ LANGUAGE plpgsql;