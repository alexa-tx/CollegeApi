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
                .Include(s => s.Subject)
                .ToListAsync();

            return Ok(schedule);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ScheduleItem item)
        {
            var group = await _context.Groups.FindAsync(item.GroupId);
            var teacher = await _context.TeacherProfiles.FindAsync(item.TeacherProfileId);
            var subject = await _context.Subjects.FindAsync(item.SubjectId);

            if (group == null || teacher == null || subject == null)
            {
                return BadRequest("Некорректные ID: группа, преподаватель или предмет не найдены.");
            }

            // Присваиваем найденные сущности
            item.Group = group;
            item.Teacher = teacher;
            item.Subject = subject;

            _context.ScheduleItems.Add(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }
    }
}
