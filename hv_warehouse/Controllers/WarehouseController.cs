using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hv_warehouse.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace hv_warehouse.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public WarehouseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/Warehouse
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            List<Warehouse> warehouseList = new List<Warehouse>();
            var connectionString = _configuration.GetConnectionString("WarehouseDB");

            await using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            string sql = "SELECT * FROM warehouses";

            await using (var cmd = new NpgsqlCommand(sql, conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var warehouse = new Warehouse();
                    warehouse.Warehouse_id = reader["warehouse_id"].ToString();
                    warehouse.Part_id = reader["part_id"].ToString();
                    warehouse.Part_qty = Convert.ToInt32(reader["part_qty"]);
                    warehouse.Warehouse_address = reader["warehouse_address"].ToString();
                    warehouse.Warehouse_location = reader["warehouse_location"].ToString();
                    warehouseList.Add(warehouse);
                }
            return Ok(warehouseList);
        }

        // GET: api/Warehouse/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return "value";
        }

        // POST: api/Warehouse
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Warehouse/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
