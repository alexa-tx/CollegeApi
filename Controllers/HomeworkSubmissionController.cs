using CollegeApi.Data;
using CollegeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            _context.HomeworkSubmissions.Add(submission);
            await _context.SaveChangesAsync();
            return Ok(submission);
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetSubmissions()
        {
            var submissions = await _context.HomeworkSubmissions
                .Include(s => s.Homework)
                .Include(s => s.StudentProfile)
                .ToListAsync();
            return Ok(submissions);
        }
    }
}
