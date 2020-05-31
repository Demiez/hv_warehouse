using System;

namespace hv_warehouse.Models
{
    public class ProductionModificationReport
    {
        public string PartId { get; set; }
        public string PartName { get; set; }
        public int? TotalShipments { get; set; }
        public double? AverageShipments { get; set; }
        public double? AverageRequired { get; set; }
        public double? ModifyProduction { get; set; }
    }
}
