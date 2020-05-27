using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hv_warehouse.OldModels
{
    public class Warehouse
    {
        public string Warehouse_id { get; set; }
        public string Part_id { get; set; }
        public int Part_qty { get; set; }
        public string Warehouse_address { get; set; }
        public string Warehouse_location { get; set; }
    }
}
