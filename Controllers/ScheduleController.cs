using CollegeApi.Data;
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

        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetGroupSchedule(int groupId)
        {
            var schedule = await _context.Schedules
                .Where(s => s.GroupId == groupId)
                .Include(s => s.Course)
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.Time)
                .ToListAsync();

            return Ok(schedule);
        }
    }
}
