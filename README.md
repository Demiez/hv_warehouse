# High Voltage Warehouse API

### There is a company of high voltage parts with production at the enterprise level. Feeds of parts, which come to the warehouse contain: high voltage part name, code, date feed, quantity of parts (for simplification). There is also an information about the shipment of hv_parts from the warehouse to customers: identity code and name, date of shipment, quantity of parts, customer. Separate info is also stored about each customer with mandatiry info about address and contact phone number.

### Already implemented:
1. DB with use of PostgreSQL, funtions, triggers, procedure (see backup and scripts folders)
2. CRUD operations for feeds and shipments of details.
3. Search, sort and filter data by the most commonly used fields.
3. Logging of queries to the database (query statistics).
4. Ability to use a free query to the database.
5. Review of the state of the warehouse: code and name of the finished product, the total volume of parts received, the total volume of shipped parts and the volume of parts remaining at the warehouse (report, computed field).
6. Automation task of production planning: taking into account the average volume of parts purchased in the last 6 months, lets assume that during next 6 months parts will  be shipped at some average volume; so it is necessary to calculate the modification of monthly production taking into account the stock, which still remains at the warehouse.
7. Invoice report with information about the customer and shipped parts.
8. Report of feed-shipment of parts to and from the warehouse according to the specified month.