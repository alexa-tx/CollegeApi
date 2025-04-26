using CollegeApi.Data;
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
            var groups = await _context.Groups.Include(g => g.Students).ToListAsync();
            return Ok(groups);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            foreach (var student in group.Students)
            {
                var studentProfile = await _context.StudentProfiles.FindAsync(student.Id);
                if (studentProfile != null)
                {
                    studentProfile.GroupId = group.Id;
                    _context.StudentProfiles.Update(studentProfile);
                }
            }

            await _context.SaveChangesAsync();

            return Ok(group);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return NotFound();

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return Ok("Группа удалена");
        }
    }
}
