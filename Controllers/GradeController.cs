using CollegeApi.Data;
using CollegeApi.DTOs;
using CollegeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GradeController : ControllerBase
    {
        private readonly CollegeDbContext _context;

        public GradeController(CollegeDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")] // Swagger отрисует поля
        public async Task<IActionResult> CreateGrade([FromForm] GradeForm form)
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

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetGradesByStudent(int studentId)
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

        [HttpGet("subject/{subjectId}")]
        public async Task<IActionResult> GetGradesBySubject(int subjectId)
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
