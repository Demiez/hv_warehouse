CREATE PROCEDURE shipment_delete (_id integer)
LANGUAGE SQL
AS 
$BODY$
	DELETE FROM shipments
    WHERE shipment_id = _id;   
$BODY$;