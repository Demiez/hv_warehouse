-- INSERT INTO feeds (part_id, part_qty, feed_date)
-- VALUES
-- 	('RES-201744', 5, NOW());
	
CREATE PROCEDURE feed_insert (_id varchar(10), _qty integer)
LANGUAGE SQL
AS $BODY$
    INSERT INTO feeds (part_id, part_qty, feed_date)
    VALUES(_id, _qty, NOW());   
$BODY$;