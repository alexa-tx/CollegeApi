using CollegeApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly CollegeDbContext _context;

        public TeacherController(CollegeDbContext context)
        {
            _context = context;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Не удалось найти пользователя.");
            }

            var teacher = await _context.TeacherProfiles
                .Include(t => t.Courses)
                .FirstOrDefaultAsync(t => t.User.Username == username);

            if (teacher == null)
                return NotFound("Профиль преподавателя не найден");

            return Ok(teacher);
        }


        [HttpPost("assign-course")]
        public async Task<IActionResult> AssignCourse([FromBody] int courseId)
        {
            var username = User.Identity?.Name;
            var teacher = await _context.TeacherProfiles.Include(t => t.Courses).FirstOrDefaultAsync(t => t.User.Username == username);
            var course = await _context.Courses.FindAsync(courseId);

            if (teacher == null || course == null)
                return NotFound("Преподаватель или курс не найден");

            teacher.Courses.Add(course);
            await _context.SaveChangesAsync();

            return Ok("Курс успешно добавлен преподавателю");
        }
    }
}
