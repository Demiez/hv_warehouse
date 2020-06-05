-- INSERT INTO feeds (part_id, part_qty, feed_date)
-- VALUES
-- 	('RES-201744', 5, NOW());
	
CREATE PROCEDURE feed_update (_id int, _partid varchar(10), _partqty integer)
LANGUAGE SQL
AS 
$BODY$
    UPDATE feeds 
	SET part_id = _partid, part_qty = _partqty
	WHERE feed_id = _id;
$BODY$;