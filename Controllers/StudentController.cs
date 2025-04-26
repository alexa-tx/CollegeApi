    using CollegeApi.Data;
    using CollegeApi.Models;
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
                var student = await _context.StudentProfiles
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

                var student = await _context.StudentProfiles
                    .Include(s => s.Enrollments)
                    .FirstOrDefaultAsync(s => s.User.Username == username);

                var course = await _context.Courses.FindAsync(courseId);

                if (student == null || course == null)
                    return NotFound("Студент или курс не найден");

                if (student.Enrollments.Any(e => e.CourseId == courseId))
                    return BadRequest("Вы уже записаны на этот курс");

                var enrollment = new Enrollment
                {
                    StudentProfileId = student.Id,
                    CourseId = course.Id,
                    EnrolledAt = DateTime.UtcNow
                };

                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();

                return Ok("Вы успешно записались на курс");
            }

        }
    }
