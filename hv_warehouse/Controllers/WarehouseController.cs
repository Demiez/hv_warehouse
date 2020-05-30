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
    }
}
