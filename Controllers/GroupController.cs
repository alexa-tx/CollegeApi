using CollegeApi.Data;
using CollegeApi.DTOs;
using CollegeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace CollegeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [SwaggerTag("Управление учебными группами")]
    public class GroupController : ControllerBase
    {
        private readonly CollegeDbContext _context;

        public GroupController(CollegeDbContext context)
        {
            _context = context;
        }
        /// <summary>Получить все группы</summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Получить все группы", Description = "Возвращает список всех учебных групп с их студентами")]
        public async Task<IActionResult> GetAll()
        {
            var groups = await _context.Groups
                .Include(g => g.Students)
                .ToListAsync();
            return Ok(groups);
        }
        /// <summary>Создать новую группу</summary>
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [SwaggerOperation(Summary = "Создать группу", Description = "Создает новую группу и назначает студентов в нее")]
        public async Task<IActionResult> Create([FromForm, SwaggerParameter("Форма с данными группы")] GroupForm form)
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
        /// <summary>Удалить группу</summary>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Удалить группу", Description = "Удаляет группу по ID")]
        public async Task<IActionResult> Delete([SwaggerParameter("ID группы")] int id)
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
    