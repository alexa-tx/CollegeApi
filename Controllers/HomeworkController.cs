using CollegeApi.Models;
using CollegeApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeworkController : ControllerBase
    {
        private readonly CollegeDbContext _context;

        public HomeworkController(CollegeDbContext context)
        {
            _context = context;
        }

        // POST /api/Homework - создание домашнего задания
        [HttpPost]
        public async Task<IActionResult> CreateHomework([FromBody] Homework homework)
        {
            if (homework == null)
            {
                return BadRequest("Домашнее задание не может быть пустым.");
            }

            // Проверка наличия курса в базе
            var course = await _context.Courses.FindAsync(homework.CourseId);
            if (course == null)
            {
                return NotFound("Курс не найден.");
            }

            // Присваиваем найденный курс объекту Homework
            homework.Course = course;

            _context.Homeworks.Add(homework);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Домашнее задание успешно создано", homework });
        }
    }
}
