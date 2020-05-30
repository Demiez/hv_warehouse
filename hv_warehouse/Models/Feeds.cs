using System;
using System.Text.Json.Serialization;

namespace hv_warehouse.Models
{
    public partial class Feeds
    {
        public int FeedId { get; set; }
        public string PartId { get; set; }
        public int? PartQty { get; set; }
        public DateTime FeedDate { get; set; }

        [JsonIgnore]
        public virtual Parts Part { get; set; }
    }
}
