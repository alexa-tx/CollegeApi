using CollegeApi.Data;
using CollegeApi.DTOs;
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

        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetByGroup(int groupId)
        {
            var schedule = await _context.ScheduleItems
                .Where(s => s.GroupId == groupId)
                .Include(s => s.Group)
                .Include(s => s.Teacher)
                .Include(s => s.Subject)
                .ToListAsync();

            if (!schedule.Any())
                return NotFound("Расписание для данной группы не найдено.");

            return Ok(schedule);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Create([FromForm] ScheduleForm form)
        {
            var group = await _context.Groups.FindAsync(form.GroupId);
            var teacher = await _context.TeacherProfiles.FindAsync(form.TeacherProfileId);
            var subject = await _context.Subjects.FindAsync(form.SubjectId);

            if (group == null || teacher == null || subject == null)
            {
                return BadRequest("Некорректные ID: группа, преподаватель или предмет не найдены.");
            }

            var item = new ScheduleItem
            {
                StartTime = form.StartTime,
                EndTime = form.EndTime,
                GroupId = form.GroupId,
                TeacherProfileId = form.TeacherProfileId,
                SubjectId = form.SubjectId,
                Group = group,
                Teacher = teacher,
                Subject = subject
            };

            _context.ScheduleItems.Add(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Update(int id, [FromForm] ScheduleForm form)
        {
            var existing = await _context.ScheduleItems.FindAsync(id);
            if (existing == null)
                return NotFound("Расписание не найдено.");

            existing.StartTime = form.StartTime;
            existing.EndTime = form.EndTime;
            existing.GroupId = form.GroupId;
            existing.TeacherProfileId = form.TeacherProfileId;
            existing.SubjectId = form.SubjectId;

            _context.ScheduleItems.Update(existing);
            await _context.SaveChangesAsync();

            return Ok(existing);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var scheduleItem = await _context.ScheduleItems.FindAsync(id);
            if (scheduleItem == null)
                return NotFound("Расписание не найдено.");

            _context.ScheduleItems.Remove(scheduleItem);
            await _context.SaveChangesAsync();

            return Ok("Расписание удалено.");
        }
    }
}
