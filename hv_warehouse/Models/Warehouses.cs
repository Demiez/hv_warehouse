using System;
using System.Collections.Generic;

namespace hv_warehouse.Models
{
    public partial class Warehouses
    {
        public string WarehouseId { get; set; }
        public string PartId { get; set; }
        public int? PartQty { get; set; }
        public string WarehouseAddress { get; set; }
        public string WarehouseLocation { get; set; }

        public virtual Parts Part { get; set; }
    }
}
