using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using hv_warehouse.Models;
using System.Data.SqlClient; // to eneable SqlParameter
using System.Linq;

namespace hv_warehouse.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly warehouse_dbContext _context;

        public FeedController(warehouse_dbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllFeeds()
        {
            string sql = "SELECT * FROM feeds";

            var feedList = await _context.Feeds.FromSqlRaw(sql)
                .OrderByDescending(feed => feed.FeedDate)
                .ToListAsync();

            return Ok(feedList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeed(int id)
        {
            string sql = "SELECT * FROM feeds WHERE feed_id = {0}";
            var feed = await _context.Feeds.FromSqlRaw(sql, id).FirstOrDefaultAsync();
            if (feed == null)
            {
                return NotFound();
            }
            return Ok(feed);
        }

        [HttpPost("new")]
        public async Task<IActionResult> AddFeed(
            [FromQuery] string partId,
            [FromQuery] int partQty
            )
        {
            var result = await _context.Database.ExecuteSqlInterpolatedAsync($"CALL feed_insert({partId}, {partQty})");
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
        public async Task<IActionResult> UpdateFeed(
                    [FromRoute] int id,
                    [FromQuery] string partId,
                    [FromQuery] int partQty
                    )
        {
            var feed = await _context.Feeds.FindAsync(id);
            if (feed == null || feed.PartQty == partQty)
                return NotFound();

            if (feed.PartQty > partQty)
            {
                var number = feed.PartQty - partQty;
                var updatedFeed01 = await _context.Database.ExecuteSqlInterpolatedAsync($"CALL feed_update({id}, {partId}, {partQty})");
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL warehouse_after_feed_update({partId}, {-number})");
                return Ok(updatedFeed01);
            }

            var partNumber = partQty - feed.PartQty;
            var updatedFeed = await _context.Database.ExecuteSqlInterpolatedAsync($"CALL feed_update({id}, {partId}, {partQty})");
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL warehouse_after_feed_update({partId}, {partNumber})");
            return Ok(updatedFeed);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeed(
                    [FromRoute] int id
                    )
        {
            var feed = await _context.Feeds.FindAsync(id);
            if (feed == null)
                return NotFound();

            var deletedRows = await _context.Database.ExecuteSqlInterpolatedAsync($"CALL feed_delete({id})");
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL warehouse_after_feed_update({feed.PartId}, {-feed.PartQty})");
            return Ok(deletedRows);
        }

        /*
        [HttpGet("with_part_name")]
        public async Task<IActionResult> GetAsyncPartNames()
        {
            List<FeedPartName> feedList = new List<FeedPartName>();
            var connectionString = _configuration.GetConnectionString("WarehouseDB");

            await using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            string sql = "SELECT f.feed_id, p.part_name, f.part_qty, f.feed_date FROM feeds f JOIN parts p ON f.part_id = p.part_id; ";

            await using (var cmd = new NpgsqlCommand(sql, conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var feed = new FeedPartName();

                    feed.Feed_id = Convert.ToInt32(reader["feed_id"]);
                    feed.Part_name = reader["part_name"].ToString();
                    feed.Part_qty = Convert.ToInt32(reader["part_qty"]);
                    feed.Feed_date = Convert.ToDateTime(reader["feed_date"]);
                    feedList.Add(feed);
                }
            return Ok(feedList);
        }*/
    }
}
