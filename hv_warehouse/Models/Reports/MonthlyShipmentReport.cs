using System;

namespace hv_warehouse.Models
{
    public class MonthlyShipmentReport
    {
        public string PartId { get; set; }
        public string PartName { get; set; }
        public int? TotalShipments { get; set; }
        public DateTime ShipmentDate { get; set; }
    }
}
