using System;
using System.Text.Json.Serialization;

namespace hv_warehouse.Models
{
    public partial class Shipments
    {
        public int ShipmentId { get; set; }
        public string PartId { get; set; }
        public DateTime ShipmentDate { get; set; }
        public int? ShipmentQty { get; set; }
        public int? CustomerId { get; set; }
        
        [JsonIgnore]
        public virtual Customers Customer { get; set; }
        [JsonIgnore]
        public virtual Parts Part { get; set; }
    }
}
