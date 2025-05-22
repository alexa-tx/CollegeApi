using CollegeApi.Data;
using CollegeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CollegeApi.DTOs;

namespace CollegeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeworkSubmissionController : ControllerBase
    {
        private readonly CollegeDbContext _context;

        public HomeworkSubmissionController(CollegeDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitHomework([FromBody] HomeworkSubmission submission)
        {
            var homework = await _context.Homeworks.FindAsync(submission.HomeworkId);
            var student = await _context.StudentProfiles.FindAsync(submission.StudentProfileId);

            if (homework == null || student == null)
                return BadRequest("Неверный ID домашнего задания или студента.");

            submission.Homework = homework;
            submission.StudentProfile = student;
            submission.SubmissionDate = DateTime.UtcNow;

            _context.HomeworkSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Домашняя работа отправлена",
                submissionId = submission.Id
            });
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetSubmissions()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var teacher = await _context.TeacherProfiles
                .Include(t => t.TeacherSubjects)
                .FirstOrDefaultAsync(t => t.UserId == userId);

            if (teacher == null)
                return Unauthorized("Профиль преподавателя не найден.");

            var subjectIds = teacher.TeacherSubjects.Select(ts => ts.SubjectId).ToList();

            var submissions = await _context.HomeworkSubmissions
                .Include(s => s.Homework)
                    .ThenInclude(h => h.Subject)
                .Include(s => s.StudentProfile)
                .Where(s => s.Homework != null && subjectIds.Contains(s.Homework.SubjectId))
                .Select(s => new HomeworkSubmissionDto
                {
                    Id = s.Id,
                    StudentName = s.StudentProfile!.FullName,
                    HomeworkTitle = s.Homework!.Title,
                    SubmissionDate = s.SubmissionDate,
                    Content = s.Content,
                    Grade = s.Grade,
                    Comment = s.Comment
                })
                .ToListAsync();

            return Ok(submissions);
        }
    }
}
