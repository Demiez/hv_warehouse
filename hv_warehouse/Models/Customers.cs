using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace hv_warehouse.Models
{
    public partial class Customers
    {
        public Customers()
        {
            Shipments = new HashSet<Shipments>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }

        [JsonIgnore]
        public virtual ICollection<Shipments> Shipments { get; set; }
    }
}
