CREATE OR REPLACE FUNCTION increase_wh_items()
RETURNS TRIGGER AS
$BODY$
BEGIN
UPDATE warehouses 
SET part_qty = part_qty + new.part_qty 
WHERE part_id = new.part_id;
RETURN NEW;
END;
$BODY$
LANGUAGE plpgsql;

CREATE TRIGGER trig_insert
AFTER INSERT ON feeds
FOR EACH ROW
EXECUTE PROCEDURE increase_wh_items();