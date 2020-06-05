CREATE PROCEDURE shipment_insert(_id varchar(10), _date varchar(29),  _qty integer, _custid integer)
LANGUAGE SQL
AS $BODY$
    INSERT INTO shipments (part_id, shipment_date, shipment_qty, customer_id)
    VALUES(_id, TO_DATE(_date, 'YYYY-MM-DD'), _qty, _custid);
$BODY$;