using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using hv_warehouse.OldModels;

namespace hv_warehouse.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FeedController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: Feed
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            List<Feed> feedList = new List<Feed>();
            var connectionString = _configuration.GetConnectionString("WarehouseDB");

            await using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            string sql = "SELECT * FROM feeds";

            await using (var cmd = new NpgsqlCommand(sql, conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var feed = new Feed();
                    feed.Feed_id = Convert.ToInt32(reader["feed_id"]);
                    feed.Part_id = reader["part_id"].ToString();
                    feed.Part_qty = Convert.ToInt32(reader["part_qty"]);
                    feed.Feed_date = Convert.ToDateTime(reader["feed_date"]);
                    feedList.Add(feed);
                }
            return Ok(feedList);
        }

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
        }

        // GET: api/Feed/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Feed
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Feed/5
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
