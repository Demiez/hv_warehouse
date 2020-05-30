using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hv_warehouse.Models;
using System.Linq;
using NpgsqlTypes;

namespace hv_warehouse.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly warehouse_dbContext _context;

        public ShipmentController(warehouse_dbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllShipments()
        {
            string sql = "SELECT * FROM shipments";

            var shipmentList = await _context.Shipments.FromSqlRaw(sql)
                .ToListAsync();

            return Ok(shipmentList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShipment(int id)
        {
            string sql = "SELECT * FROM shipments WHERE shipment_id = {0}";
            var shipment = await _context.Shipments.FromSqlRaw(sql, id).FirstOrDefaultAsync();
            if (shipment == null)
            {
                return NotFound();
            }
            return Ok(shipment);
        }

        [HttpPost("new")]
        public async Task<IActionResult> AddShipment(
            [FromQuery] string partId,
            [FromQuery] int shipmentQty,
            [FromQuery] DateTime shipmentDate,
            [FromQuery] int customerId
            )
        {
            string sqlDate = shipmentDate.ToString("yyyy-MM-dd");
            var result = await _context.Database.ExecuteSqlInterpolatedAsync($"CALL shipment_insert({partId}, {sqlDate}, {shipmentQty}, {customerId})");
            if (result == -1)
            {
                return Ok("Added successfully");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateShipment(
                    [FromRoute] int id,
                    [FromQuery] string partId,
                    [FromQuery] int shipmentQty,
                    [FromQuery] DateTime shipmentDate
                    )
        {
            string sql = "SELECT * FROM shipments WHERE shipment_id = {0}";
            string sqlDate = shipmentDate.ToString("yyyy-MM-dd");

            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null || shipment.ShipmentQty == shipmentQty)
                return NotFound();

            if (shipment.ShipmentQty > shipmentQty)
            {
                var number = shipment.ShipmentQty - shipmentQty;
                var query1 = await _context.Database.ExecuteSqlInterpolatedAsync($"CALL shipment_update({id}, {partId}, {shipmentQty}, {sqlDate})");
                if (query1 == -1)
                {
                    await _context.Database.ExecuteSqlInterpolatedAsync($"CALL warehouse_after_shipment_update({partId}, {-number})");
                    _context.Entry(shipment).Reload();
                    var result1 = await _context.Shipments.FromSqlRaw(sql, id).FirstAsync();
                    return Ok(result1);
                }
                else
                {
                    return NotFound();
                }
            }

            var partNumber = shipmentQty - shipment.ShipmentQty;
            var query2 = await _context.Database.ExecuteSqlInterpolatedAsync($"CALL shipment_update({id}, {partId}, {shipmentQty}, {sqlDate})");
            if (query2 != -1)
            {
                return NotFound();
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL warehouse_after_shipment_update({partId}, {partNumber})");
            _context.Entry(shipment).Reload();
            var result2 = await _context.Shipments.FromSqlRaw(sql, id).FirstOrDefaultAsync();
            return Ok(result2);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipment(
                    [FromRoute] int id
                    )
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null)
                return NotFound();

            var deletedRows = await _context.Database.ExecuteSqlInterpolatedAsync($"CALL shipment_delete({id})");
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL warehouse_after_shipment_update({shipment.PartId}, {-shipment.ShipmentQty})");
            return Ok(deletedRows);
        }
    }
}
