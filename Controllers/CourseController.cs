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
    public class CourseController : ControllerBase
    {
        private readonly CollegeDbContext _context;

        public CourseController(CollegeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _context.Courses
                .Include(c => c.Teacher)
                .ToListAsync();

            return Ok(courses);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [Consumes("application/x-www-form-urlencoded")] // <-- Swagger отрисует форму
        public async Task<IActionResult> Create([FromForm] CourseForm form)
        {
            // Проверяем, что преподаватель существует
            var teacher = await _context.TeacherProfiles.FindAsync(form.TeacherProfileId);
            if (teacher == null)
                return NotFound("Преподаватель с таким ID не найден.");

            // Собираем сущность
            var course = new Course
            {
                Title = form.Title,
                Description = form.Description,
                TeacherProfileId = form.TeacherProfileId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return Ok(course);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            if (course.Enrollments.Any())
                return BadRequest("Невозможно удалить курс, пока на него записаны студенты.");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return Ok("Курс удален.");
        }
    }
}
