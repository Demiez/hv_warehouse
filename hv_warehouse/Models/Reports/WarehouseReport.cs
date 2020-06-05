namespace hv_warehouse.Models
{
    public class WarehouseReport
    {
        public string PartId { get; set; }
        public string PartName { get; set; }
        public int? TotalFeeds { get; set; }
        public int? TotalShipments { get; set; }
        public int? WarehouseQty { get; set; }
    }
}
