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
    [SwaggerTag("Управление учебными дисциплинами колледжа")]
    public class SubjectController : ControllerBase
    {
        private readonly CollegeDbContext _context;

        public SubjectController(CollegeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Список учебных дисциплин", Description = "Получение списка учебных дисциплин колледжа")]
        public async Task<IActionResult> GetAll()
        {
            var subjects = await _context.Subjects.ToListAsync();
            return Ok(subjects);
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [SwaggerOperation(Summary = "Создание учебной дисциплины", Description = "Создание учебной дисциплины колледжа")]
        public async Task<IActionResult> Create([FromForm] SubjectForm form)
        {
            var subject = new Subject
            {
                Name = form.Name
            };

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return Ok(subject);
        }
    }
}
