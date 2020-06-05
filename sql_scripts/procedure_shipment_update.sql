CREATE PROCEDURE shipment_update (_id int, _partid varchar(10), _qty integer, _date varchar(29))
LANGUAGE SQL
AS 
$BODY$
    UPDATE shipments 
	SET part_id = _partid, shipment_date = TO_DATE(_date, 'YYYY-MM-DD'), shipment_qty = _qty
	WHERE shipment_id = _id;
$BODY$;