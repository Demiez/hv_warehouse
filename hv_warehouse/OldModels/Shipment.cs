using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hv_warehouse.OldModels
{
    public class Shipment
    {
        public int Shipment_id { get; set; }
        public string Part_id { get; set; }
        public DateTime Shipment_date { get; set; }
        public int Shipment_qty { get; set; }
        public int Customer_id { get; set; }
    }
}
