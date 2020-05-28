using System;
using System.Collections.Generic;

namespace hv_warehouse.Models
{
    public partial class Parts
    {
        public Parts()
        {
            Feeds = new HashSet<Feeds>();
            Shipments = new HashSet<Shipments>();
            Warehouses = new HashSet<Warehouses>();
        }

        public string PartId { get; set; }
        public string PartName { get; set; }
        public string PartMfr { get; set; }
        public decimal? PartPrice { get; set; }
        public string PartDesc { get; set; }

        public virtual ICollection<Feeds> Feeds { get; set; }
        public virtual ICollection<Shipments> Shipments { get; set; }
        public virtual ICollection<Warehouses> Warehouses { get; set; }
    }
}
