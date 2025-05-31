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
    [SwaggerTag("Управление оценками")]
    public class GradeController : ControllerBase
    {
        private readonly CollegeDbContext _context;

        public GradeController(CollegeDbContext context)
        {
            _context = context;
        }
        /// <summary>Создать новую оценку</summary>
        /// <param name="form">Данные оценки</param>
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [SwaggerOperation(Summary = "Создать оценку", Description = "Создает новую оценку студенту по предмету")]
        public async Task<IActionResult> CreateGrade([FromForm, SwaggerParameter("Форма с данными оценки")] GradeForm form)
        {
            if (!await _context.StudentProfiles.AnyAsync(s => s.Id == form.StudentProfileId))
                return NotFound("Студент не найден.");

            if (!await _context.Subjects.AnyAsync(s => s.Id == form.SubjectId))
                return NotFound("Предмет не найден.");

            var grade = new Grade
            {
                StudentProfileId = form.StudentProfileId,
                SubjectId = form.SubjectId,
                Value = form.Value,
                Comment = form.Comment,
                GradeType = form.GradeType,
                CreatedAt = DateTime.UtcNow
            };

            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();

            return Ok(grade);
        }
        /// <summary>Получить оценки студента</summary>
        [HttpGet("student/{studentId}")]
        [SwaggerOperation(Summary = "Оценки по студенту", Description = "Получает все оценки конкретного студента")]
        public async Task<IActionResult> GetGradesByStudent([SwaggerParameter("ID студента")] int studentId)
        {
            var grades = await _context.Grades
                .Where(g => g.StudentProfileId == studentId)
                .Include(g => g.Subject)
                .ToListAsync();

            if (!grades.Any())
                return NotFound("Оценки для этого студента не найдены.");

            var average = grades.Average(g => g.Value);

            return Ok(new
            {
                Grades = grades,
                AverageGrade = average
            });
        }
        /// <summary>Получить оценки по предмету</summary>
        [HttpGet("subject/{subjectId}")]
        [SwaggerOperation(Summary = "Оценки по предмету", Description = "Получает все оценки по заданному предмету")]
        public async Task<IActionResult> GetGradesBySubject([SwaggerParameter("ID предмета")] int subjectId)
        {
            var grades = await _context.Grades
                .Where(g => g.SubjectId == subjectId)
                .Include(g => g.StudentProfile)
                .ToListAsync();

            if (!grades.Any())
                return NotFound("Оценки для этого предмета не найдены.");

            var average = grades.Average(g => g.Value);

            return Ok(new
            {
                Grades = grades,
                AverageGrade = average
            });
        }
    }
}
