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
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> CreateHomework([FromForm] HomeworkForm form)
        {
            var subject = await _context.Subjects.FindAsync(form.SubjectId);
            if (subject == null)
                return NotFound("Предмет не найден.");

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
                homework
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHomework(int id)
        {
            var homework = await _context.Homeworks.FindAsync(id);
            if (homework == null)
                return NotFound("Домашнее задание не найдено.");

            _context.Homeworks.Remove(homework);
            await _context.SaveChangesAsync();

            return Ok("Домашнее задание удалено.");
        }

        [HttpDelete("expired")]
        public async Task<IActionResult> DeleteExpiredHomeworks()
        {
            var now = DateTime.UtcNow;
            var expiredHomeworks = await _context.Homeworks
                .Where(h => h.DueDate < now)
                .ToListAsync();

            if (!expiredHomeworks.Any())
                return Ok("Нет просроченных домашних заданий.");

            _context.Homeworks.RemoveRange(expiredHomeworks);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Удалены просроченные домашние задания.",
                count = expiredHomeworks.Count
            });
        }
    }
}
