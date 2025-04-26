using CollegeApi.Data;
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
        public async Task<IActionResult> CreateGrade([FromBody] Grade grade)
        {
            if (!await _context.StudentProfiles.AnyAsync(s => s.Id == grade.StudentProfileId))
                return NotFound("Студент не найден.");

            if (!await _context.Courses.AnyAsync(c => c.Id == grade.CourseId))
                return NotFound("Курс не найден.");

            grade.CreatedAt = DateTime.UtcNow; // Защита от "кривых" дат из клиента
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();

            return Ok(grade);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetGradesByStudent(int studentId)
        {
            var grades = await _context.Grades
                .Where(g => g.StudentProfileId == studentId)
                .Include(g => g.Course)
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

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetGradesByCourse(int courseId)
        {
            var grades = await _context.Grades
                .Where(g => g.CourseId == courseId)
                .Include(g => g.StudentProfile)
                .ToListAsync();

            if (!grades.Any())
                return NotFound("Оценки для этого курса не найдены.");

            var average = grades.Average(g => g.Value);

            return Ok(new
            {
                Grades = grades,
                AverageGrade = average
            });
        }
    }
}
