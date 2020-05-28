using System;
using System.Collections.Generic;

namespace hv_warehouse.Models
{
    public partial class Shipments
    {
        public int ShipmentId { get; set; }
        public string PartId { get; set; }
        public DateTime ShipmentDate { get; set; }
        public int? ShipmentQty { get; set; }
        public int? CustomerId { get; set; }

        public virtual Customers Customer { get; set; }
        public virtual Parts Part { get; set; }
    }
}
