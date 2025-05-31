using CollegeApi.Data;
using CollegeApi.DTOs;
using CollegeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Управление курсами")]
    public class CourseController : ControllerBase
    {
        private readonly CollegeDbContext _context;

        public CourseController(CollegeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Получить все курсы", Description = "Возвращает список всех доступных курсов вместе с информацией о преподавателе")]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _context.Courses
                .Include(c => c.Teacher)
                .ToListAsync();

            return Ok(courses);
        }
        /// <summary>
        /// Создать новый курс
        /// </summary>
        /// <param name="form">Данные курса</param>
        /// <returns>Созданный курс</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [Consumes("application/x-www-form-urlencoded")]
        [SwaggerOperation(Summary = "Создать курс", Description = "Позволяет администраторам и преподавателям создавать новые курсы")]
        public async Task<IActionResult> Create([FromForm, SwaggerParameter("Форма данных курса")] CourseForm form)
        {
            var teacher = await _context.TeacherProfiles.FindAsync(form.TeacherProfileId);
            if (teacher == null)
                return NotFound("Преподаватель с таким ID не найден.");

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
        /// <summary>
        /// Удалить курс
        /// </summary>
        /// <param name="id">ID курса</param>
        /// <returns>Сообщение об удалении</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Удалить курс", Description = "Удаляет курс, если на него не записаны студенты")]
        public async Task<IActionResult> Delete([SwaggerParameter("Идентификатор курса для удаления")] int id)
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
