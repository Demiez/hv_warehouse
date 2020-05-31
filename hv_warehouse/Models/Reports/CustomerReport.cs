using System;

namespace hv_warehouse.Models
{
    public class CustomerReport
    {
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public string PartName { get; set; }
        public int ShipmentQty { get; set; }
        public DateTime ShipmentDate { get; set; }
    }
}
