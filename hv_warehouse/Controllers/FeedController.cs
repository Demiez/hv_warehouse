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
        public async Task<IActionResult> GetAllFeeds(
            [FromQuery] string sortField,
            [FromQuery] string sortOrder,
            [FromQuery] string filterString,
            [FromQuery] string searchString
            )
        {
            string sql = "SELECT * FROM feeds";
            string sorting = $"{sortField}-{sortOrder}".ToLower();
            List<Feeds> feedList = new List<Feeds>();

            switch (sorting)
            {
                case "feedid-desc":
                    feedList = await _context.Feeds.FromSqlRaw(sql).OrderByDescending(feed => feed.FeedId).ToListAsync();
                    break;
                case "partid-asc":
                    feedList = await _context.Feeds.FromSqlRaw(sql).OrderBy(feed => feed.PartId).ToListAsync();
                    break;
                case "partid-desc":
                    feedList = await _context.Feeds.FromSqlRaw(sql).OrderByDescending(feed => feed.PartId).ToListAsync();
                    break;
                case "partqty-asc":
                    feedList = await _context.Feeds.FromSqlRaw(sql).OrderBy(feed => feed.PartQty).ToListAsync();
                    break;
                case "partqty-desc":
                    feedList = await _context.Feeds.FromSqlRaw(sql).OrderByDescending(feed => feed.PartQty).ToListAsync();
                    break;
                case "feeddate-asc":
                    feedList = await _context.Feeds.FromSqlRaw(sql).OrderBy(feed => feed.FeedDate).ToListAsync();
                    break;
                case "feeddate-desc":
                    feedList = await _context.Feeds.FromSqlRaw(sql).OrderByDescending(feed => feed.FeedDate).ToListAsync();
                    break;
                default:
                    feedList = await _context.Feeds.FromSqlRaw(sql).OrderBy(feed => feed.FeedId).ToListAsync();
                    break;
            }

            if (!String.IsNullOrEmpty(filterString))
            {
                var number = Int32.Parse(filterString.Substring(1));
                var op = filterString.Substring(0,1);
                if (op == "<")
                {
                    feedList = feedList.Where(feed => feed.PartQty < number).ToList();
                }
                else if (op == ">")
                {
                    feedList = feedList.Where(feed => feed.PartQty > number).ToList();
                }
                else
                {
                    feedList = feedList.Where(feed => feed.PartQty == number).ToList();
                }
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                feedList = feedList.Where(feed => feed.PartId.Contains(searchString.ToUpper())).ToList();
            }

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
            string sql = "SELECT * FROM feeds WHERE feed_id = {0}";
            var feed = await _context.Feeds.FindAsync(id);

            if (feed == null || feed.PartQty == partQty)
                return NotFound();

            if (feed.PartQty > partQty)
            {
                var number = feed.PartQty - partQty;
                var query1 = await _context.Database.ExecuteSqlInterpolatedAsync($"CALL feed_update({id}, {partId}, {partQty})");
                if (query1 == -1)
                {
                    await _context.Database.ExecuteSqlInterpolatedAsync($"CALL warehouse_after_feed_update({partId}, {-number})");
                    _context.Entry(feed).Reload();
                    var result1 = await _context.Feeds.FromSqlRaw(sql, id).FirstAsync();
                    return Ok(result1);
                }
                else
                {
                    return NotFound();
                }
            }

            var partNumber = partQty - feed.PartQty;
            var query2 = await _context.Database.ExecuteSqlInterpolatedAsync($"CALL feed_update({id}, {partId}, {partQty})");
            if (query2 != -1)
            {
                return NotFound();
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL warehouse_after_feed_update({partId}, {partNumber})");
            _context.Entry(feed).Reload();
            var result2 = await _context.Feeds.FromSqlRaw(sql, id).FirstAsync();
            return Ok(result2);
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
    }
}
