CREATE PROCEDURE warehouse_after_shipment_update (_partid varchar(10), _partqty integer)
LANGUAGE SQL
AS 
$BODY$
    UPDATE warehouses 
	SET part_qty = part_qty - _partqty
	WHERE part_id = _partid;
$BODY$;