using CollegeApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    public class StudentController : ControllerBase
    {
        private readonly CollegeDbContext _context;

        public StudentController(CollegeDbContext context)
        {
            _context = context;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var username = User.Identity?.Name;
            var student = await _context.Students
                .Include(s => s.Group)
                .FirstOrDefaultAsync(s => s.User.Username == username);

            if (student == null)
                return NotFound("Профиль студента не найден");

            return Ok(student);
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollToCourse([FromBody] int courseId)
        {
            var username = User.Identity?.Name;
            var student = await _context.Students.Include(s => s.Courses).FirstOrDefaultAsync(s => s.User.Username == username);
            var course = await _context.Courses.FindAsync(courseId);

            if (student == null || course == null)
                return NotFound("Студент или курс не найден");

            student.Courses.Add(course);
            await _context.SaveChangesAsync();

            return Ok("Вы успешно записались на курс");
        }
    }
}
