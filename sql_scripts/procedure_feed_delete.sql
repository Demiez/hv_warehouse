CREATE PROCEDURE feed_delete (_id integer)
LANGUAGE SQL
AS 
$BODY$
	DELETE FROM feeds
    WHERE feed_id = _id;   
$BODY$;