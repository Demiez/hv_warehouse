using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hv_warehouse.Models
{
    public class Feed
    {
        public int Feed_id { get; set; }
        public string Part_id { get; set; }
        public int Part_qty { get; set; }
        public DateTime Feed_date { get; set; }
    }
}
