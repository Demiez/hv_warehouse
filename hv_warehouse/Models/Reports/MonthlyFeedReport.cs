using System;

namespace hv_warehouse.Models
{
    public class MonthlyFeedReport
    {
        public string PartId { get; set; }
        public string PartName { get; set; }
        public int? TotalFeeds { get; set; }
        public DateTime FeedDate { get; set; }
    }
}
