using CollegeApi.Data;
using CollegeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly CollegeDbContext _context;

        public NewsController(CollegeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNews()
        {
            var news = await _context.News.OrderByDescending(n => n.Date).ToListAsync();
            return Ok(news);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> PostNews([FromBody] NewsItem item)
        {
            item.Date = DateTime.UtcNow;
            _context.News.Add(item);
            await _context.SaveChangesAsync();
            return Ok("Новость опубликована");
        }
    }
}
