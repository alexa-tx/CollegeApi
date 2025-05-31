using CollegeApi.Data;
using CollegeApi.DTOs;
using CollegeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace CollegeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Управление домашними заданиями")]
    public class HomeworkController : ControllerBase
    {
        private readonly CollegeDbContext _context;

        public HomeworkController(CollegeDbContext context)
        {
            _context = context;
        }
        /// <summary>Создать новое домашнее задание</summary>
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [SwaggerOperation(Summary = "Создание домашнего задания", Description = "Создает новое домашнее задание по предмету")]
        public async Task<IActionResult> CreateHomework([FromForm, SwaggerParameter("Форма с данными домашнего задания")] HomeworkForm form)
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
        /// <summary>Удалить домашнее задание по ID</summary>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Удаление по ID", Description = "Удаляет конкретное домашнее задание")]
        public async Task<IActionResult> DeleteHomework([SwaggerParameter("ID домашнего задания")] int id)
        {
            var homework = await _context.Homeworks.FindAsync(id);
            if (homework == null)
                return NotFound("Домашнее задание не найдено.");

            _context.Homeworks.Remove(homework);
            await _context.SaveChangesAsync();

            return Ok("Домашнее задание удалено.");
        }
        /// <summary>Удалить все просроченные домашние задания</summary>
        [HttpDelete("expired")]
        [SwaggerOperation(Summary = "Удаление просроченных", Description = "Удаляет все домашние задания с истекшим сроком сдачи")]
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
