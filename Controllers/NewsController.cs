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
        public async Task<IActionResult> GetNews()
        {
            var news = await _context.NewsPosts.OrderByDescending(n => n.DatePosted).ToListAsync();
            return Ok(news);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostNews([FromBody] NewsPost news)
        {
            news.DatePosted = DateTime.UtcNow;
            _context.NewsPosts.Add(news);
            await _context.SaveChangesAsync();
            return Ok(news);
        }
    }
}
