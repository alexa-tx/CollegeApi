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
    [Authorize(Roles = "Admin,Teacher")]
    public class TeacherSubjectController : ControllerBase
    {
        private readonly CollegeDbContext _context;

        public TeacherSubjectController(CollegeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.TeacherSubjects
                .Include(ts => ts.TeacherProfile)
                .Include(ts => ts.Subject)
                .ToListAsync();
            return Ok(list);
        }

        [HttpGet("teacher/{teacherId}")]
        public async Task<IActionResult> GetByTeacher(int teacherId)
        {
            var items = await _context.TeacherSubjects
                .Where(ts => ts.TeacherProfileId == teacherId)
                .Include(ts => ts.Subject)
                .ToListAsync();
            return Ok(items.Select(ts => ts.Subject));
        }

        [HttpGet("subject/{subjectId}")]
        public async Task<IActionResult> GetBySubject(int subjectId)
        {
            var items = await _context.TeacherSubjects
                .Where(ts => ts.SubjectId == subjectId)
                .Include(ts => ts.TeacherProfile)
                .ToListAsync();
            return Ok(items.Select(ts => ts.TeacherProfile));
        }

        
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Assign([FromForm] TeacherSubjectForm form)
        {
            if (!await _context.TeacherProfiles.AnyAsync(t => t.Id == form.TeacherProfileId))
                return NotFound("Преподаватель не найден.");

            if (!await _context.Subjects.AnyAsync(s => s.Id == form.SubjectId))
                return NotFound("Предмет не найден.");

            bool exists = await _context.TeacherSubjects
                .AnyAsync(ts => ts.TeacherProfileId == form.TeacherProfileId
                             && ts.SubjectId == form.SubjectId);
            if (exists)
                return BadRequest("Эта связь уже существует.");

            var tsEntity = new TeacherSubject
            {
                TeacherProfileId = form.TeacherProfileId,
                SubjectId = form.SubjectId
            };

            _context.TeacherSubjects.Add(tsEntity);
            await _context.SaveChangesAsync();

            return Ok(tsEntity);
        }

        [HttpDelete]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Unassign([FromForm] TeacherSubjectForm form)
        {
            var tsEntity = await _context.TeacherSubjects
                .FindAsync(form.TeacherProfileId, form.SubjectId);

            if (tsEntity == null)
                return NotFound("Связь не найдена.");

            _context.TeacherSubjects.Remove(tsEntity);
            await _context.SaveChangesAsync();
            return Ok("Связь удалена.");
        }
    }
}
