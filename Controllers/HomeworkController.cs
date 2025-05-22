using CollegeApi.Data;
using CollegeApi.DTOs;
using CollegeApi.Models;
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

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]  // для Swagger UI формы
        public async Task<IActionResult> CreateHomework([FromForm] HomeworkForm form)
        {
            // Проверяем предмет
            var subject = await _context.Subjects.FindAsync(form.SubjectId);
            if (subject == null)
                return NotFound("Предмет не найден.");

            // Собираем модель
            var homework = new Homework
            {
                Title = form.Title,
                Description = form.Description,
                DueDate = form.DueDate,
                SubjectId = form.SubjectId,
                Subject = subject,
                CreatedAt = DateTime.UtcNow
            };

            _context.Homeworks.Add(homework);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Домашнее задание успешно создано",
                homework = homework
            });
        }
    }
}
