using CollegeApi.Data;
using CollegeApi.DTOs;
using CollegeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class GroupController : ControllerBase
    {
        private readonly CollegeDbContext _context;

        public GroupController(CollegeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var groups = await _context.Groups
                .Include(g => g.Students)
                .ToListAsync();
            return Ok(groups);
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Create([FromForm] GroupForm form)
        {
            var group = new Group
            {
                Name = form.Name
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            foreach (var studentId in form.StudentProfileIds)
            {
                var student = await _context.StudentProfiles.FindAsync(studentId);
                if (student != null)
                {
                    student.GroupId = group.Id;
                    _context.StudentProfiles.Update(student);
                }
            }

            await _context.SaveChangesAsync();

            await _context.Entry(group)
                .Collection(g => g.Students)
                .Query()
                .LoadAsync();

            return Ok(group);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
                return NotFound();

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return Ok("Группа удалена");
        }
    }
}
    