CREATE OR REPLACE FUNCTION decrease_wh_items()
RETURNS TRIGGER AS
$BODY$
BEGIN
UPDATE warehouses 
SET part_qty = part_qty - new.shipment_qty 
WHERE part_id = new.part_id;
RETURN NEW;
END;
$BODY$
LANGUAGE plpgsql;

CREATE TRIGGER trig_decrease
AFTER INSERT ON shipments
FOR EACH ROW
EXECUTE PROCEDURE decrease_wh_items();