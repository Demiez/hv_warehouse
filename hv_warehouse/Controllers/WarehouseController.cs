using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using hv_warehouse.Models;
using Microsoft.EntityFrameworkCore;

namespace hv_warehouse.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly warehouse_dbContext _context;

        public WarehouseController(warehouse_dbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllWarehouses()
        {
            string sql = "SELECT * FROM warehouses";

            //string sql1 = "SELECT w.warehouse_id, p.part_name, w.part_qty, w.warehouse_address, w.warehouse_location FROM warehouses w JOIN parts p ON w.part_id = p.part_id;";

            var warehouseList = await _context.Warehouses.FromSqlRaw(sql).ToListAsync();
            return Ok(warehouseList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWarehouse(string id)
        {
            string sql = "SELECT * FROM warehouses WHERE warehouse_id = {0}";
            var warehouse = await _context.Warehouses.FromSqlRaw(sql, id).FirstOrDefaultAsync();
            if (warehouse == null)
            {
                return NotFound();
            }
            return Ok(warehouse);
        }


        /*[HttpGet("old")]
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
        }*/
    }
}
