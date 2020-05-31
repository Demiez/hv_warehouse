using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using hv_warehouse.Models;
using Microsoft.EntityFrameworkCore;

namespace hv_warehouse.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PartController : ControllerBase
    {
        private readonly warehouse_dbContext _context;

        public PartController(warehouse_dbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllParts()
        {
            string sql = "SELECT * FROM parts";

            var partList = await _context.Parts.FromSqlRaw(sql).ToListAsync();
            return Ok(partList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPart(string id)
        {
            string sql = "SELECT * FROM parts WHERE part_id = {0}";
            var part = await _context.Parts.FromSqlRaw(sql, id).FirstOrDefaultAsync();
            if (part == null)
            {
                return NotFound();
            }
            return Ok(part);
        }

        [HttpGet("sql")]
        public async Task<IActionResult> GetSQLPart(
            [FromQuery] string SQL
            )
        {
            var sqlResult = await _context.Parts.FromSqlRaw(SQL).ToListAsync();
            if (sqlResult == null)
            {
                return NotFound();
            }
            return Ok(sqlResult);
        }
    }
}
