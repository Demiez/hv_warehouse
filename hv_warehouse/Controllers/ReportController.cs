using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using hv_warehouse.Models;
using System.Data.SqlClient; // to eneable SqlParameter
using System.Linq;
using Npgsql;

namespace hv_warehouse.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly warehouse_dbContext _context;
        private readonly IConfiguration _configuration;

        public ReportController(IConfiguration configuration, warehouse_dbContext context)
        {
            _context = context;
            _configuration = configuration;
        }
        
        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomersReport()
        {
            List<CustomerReport> customersReport = new List<CustomerReport>();
            var connectionString = _configuration.GetConnectionString("WarehouseDB");

            await using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            string sql = "SELECT c.customer_name, c.customer_address, c.customer_phone, p.part_name, s.shipment_qty, s.shipment_date FROM shipments s JOIN customers c ON s.shipment_id = c.customer_id JOIN parts p ON s.part_id = p.part_id;";

            await using (var cmd = new NpgsqlCommand(sql, conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var info = new CustomerReport();

                    info.CustomerName = reader["customer_name"].ToString();
                    info.CustomerAddress = reader["customer_address"].ToString();
                    info.CustomerPhone = reader["customer_phone"].ToString();
                    info.PartName = reader["part_name"].ToString();
                    info.ShipmentQty = Convert.ToInt32(reader["shipment_qty"]);
                    info.ShipmentDate = Convert.ToDateTime(reader["shipment_date"]);
                    customersReport.Add(info);
                }
            return Ok(customersReport);
        }

        [HttpGet("warehouse-state")]
        public async Task<IActionResult> GetWarehouseState()
        {
            List<WarehouseReport> warehouseReport = new List<WarehouseReport>();
            var connectionString = _configuration.GetConnectionString("WarehouseDB");

            await using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            string sql = "SELECT DISTINCT p.part_id, p.part_name, (SELECT SUM(f.part_qty) FROM feeds f WHERE f.part_id = p.part_id) AS total_feeds, (SELECT SUM(s.shipment_qty) FROM shipments s WHERE s.part_id = p.part_id) AS total_shipments, w.part_qty AS warehouse_qty FROM parts p LEFT JOIN feeds f ON p.part_id = f.part_id LEFT JOIN shipments s ON p.part_id = s.part_id JOIN warehouses w ON p.part_id = w.part_id;";

            await using (var cmd = new NpgsqlCommand(sql, conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var info = new WarehouseReport();

                    info.PartId = reader["part_id"] is DBNull ? "N/A" : reader["part_id"].ToString();
                    info.PartName = reader["part_name"] is DBNull ? "N/A" : reader["part_name"].ToString();
                    info.TotalFeeds = reader["total_feeds"] is DBNull ? 0 : Convert.ToInt32(reader["total_feeds"]);
                    info.TotalShipments = reader["total_shipments"] is DBNull ? 0 : Convert.ToInt32(reader["total_shipments"]);
                    info.WarehouseQty = reader["warehouse_qty"] is DBNull ? 0 : Convert.ToInt32(reader["warehouse_qty"]);
                    warehouseReport.Add(info);
                }
            return Ok(warehouseReport);
        }

        [HttpGet("monthly-feed-state")]
        public async Task<IActionResult> GetMonthlyFeedState(
            [FromQuery] string month
            )
        {
            List<MonthlyFeedReport> monthlyReport = new List<MonthlyFeedReport>();
            var connectionString = _configuration.GetConnectionString("WarehouseDB");
            string currentYear = DateTime.Now.Year.ToString();

            await using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            string sql = $"SELECT DISTINCT p.part_id, p.part_name, (SELECT SUM(f.part_qty) FROM feeds f WHERE f.part_id = p.part_id) AS total_feeds, feed_date AS date FROM parts p LEFT JOIN feeds f ON p.part_id = f.part_id LEFT JOIN shipments s ON p.part_id = s.part_id JOIN warehouses w ON p.part_id = w.part_id WHERE DATE_PART('month',feed_date) = {month} AND DATE_PART('year',feed_date) = {currentYear};";

            await using (var cmd = new NpgsqlCommand(sql, conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var info = new MonthlyFeedReport();

                    info.PartId = reader["part_id"] is DBNull ? "N/A" : reader["part_id"].ToString();
                    info.PartName = reader["part_name"] is DBNull ? "N/A" : reader["part_name"].ToString();
                    info.TotalFeeds = reader["total_feeds"] is DBNull ? 0 : Convert.ToInt32(reader["total_feeds"]);
                    info.FeedDate = Convert.ToDateTime(reader["date"]);
                    monthlyReport.Add(info);
                }
            return Ok(monthlyReport);
        }

        [HttpGet("monthly-shipment-state")]
        public async Task<IActionResult> GetMonthlyShipmentState(
            [FromQuery] string month
            )
        {
            List<MonthlyShipmentReport> monthlyReport = new List<MonthlyShipmentReport>();
            var connectionString = _configuration.GetConnectionString("WarehouseDB");
            string currentYear = DateTime.Now.Year.ToString();

            await using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            string sql = $"SELECT DISTINCT p.part_id, p.part_name, (SELECT SUM(s.shipment_qty) FROM shipments s WHERE s.part_id = p.part_id) AS total_shipments, shipment_date AS date FROM parts p LEFT JOIN shipments s ON p.part_id = s.part_id WHERE DATE_PART('month',shipment_date) = {month} AND DATE_PART('year',shipment_date) = {currentYear}; ";

            await using (var cmd = new NpgsqlCommand(sql, conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var info = new MonthlyShipmentReport();

                    info.PartId = reader["part_id"] is DBNull ? "N/A" : reader["part_id"].ToString();
                    info.PartName = reader["part_name"] is DBNull ? "N/A" : reader["part_name"].ToString();
                    info.TotalShipments = reader["total_shipments"] is DBNull ? 0 : Convert.ToInt32(reader["total_shipments"]);
                    info.ShipmentDate = Convert.ToDateTime(reader["date"]);
                    monthlyReport.Add(info);
                }
            return Ok(monthlyReport);
        }
    }
}
