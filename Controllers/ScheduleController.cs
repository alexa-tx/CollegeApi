using CollegeApi.Data;
using CollegeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly CollegeDbContext _context;

        public ScheduleController(CollegeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var schedule = await _context.ScheduleItems
                .Include(s => s.Group) 
                .Include(s => s.Teacher) 
                .Include(s => s.Course)
                .ToListAsync();

            return Ok(schedule);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ScheduleItem item)
        {
            _context.ScheduleItems.Add(item);
            await _context.SaveChangesAsync();
            return Ok(item);
        }
    }
}
