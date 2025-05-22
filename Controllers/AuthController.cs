using CollegeApi.Data;
using CollegeApi.DTOs;
using CollegeApi.Interfaces;
using CollegeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CollegeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly CollegeDbContext _context;

        public AuthController(IAuthService authService, CollegeDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpPost("login")]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult Login([FromForm] LoginForm form)
        {
            if (_authService.ValidateUser(form.Username, form.Password))
            {
                var token = _authService.GenerateJwtToken(form.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Register([FromForm] RegisterForm form)
        {
            if (await _context.Users.AnyAsync(u => u.Username == form.Username))
                return BadRequest("Пользователь с таким логином уже существует");

            var user = new User
            {
                Username = form.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(form.Password),
                Role = form.Role ?? "Student"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Соберём ФИО
            var fullName = $"{form.LastName} {form.FirstName} {form.MiddleName}".Trim();

            if (user.Role == "Teacher")
            {
                var teacherProfile = new TeacherProfile
                {
                    FullName = fullName,
                    UserId = user.Id
                };
                _context.TeacherProfiles.Add(teacherProfile);
            }
            else // Student
            {
                // Найдём или создадим Default Group
                var defaultGroup = await _context.Groups.FirstOrDefaultAsync(g => g.Name == "Default Group")
                                   ?? new Group { Name = "Default Group" };

                if (defaultGroup.Id == 0)
                {
                    _context.Groups.Add(defaultGroup);
                    await _context.SaveChangesAsync();
                }

                var studentProfile = new StudentProfile
                {
                    FullName = fullName,
                    UserId = user.Id,
                    GroupId = defaultGroup.Id
                };
                _context.StudentProfiles.Add(studentProfile);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Регистрация прошла успешно" });
        }

        [HttpPost("adminOnly")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnlyAction() => Ok("Только для администратора");

        [HttpPost("teacherOnly")]
        [Authorize(Roles = "Teacher")]
        public IActionResult TeacherOnlyAction() => Ok("Только для преподавателя");

        [HttpPost("studentOnly")]
        [Authorize(Roles = "Student")]
        public IActionResult StudentOnlyAction() => Ok("Только для студента");

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var username = User.Identity?.Name;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return Ok(new { Username = username, Role = role });
        }
    }

    // Старая модель логина остаётся без изменений
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
