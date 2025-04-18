using CollegeApi.Models;
using CollegeApi.Data;
using CollegeApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (_authService.ValidateUser(request.Username, request.Password))
            {
                var token = _authService.GenerateJwtToken(request.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (existingUser != null)
                return BadRequest("Пользователь с таким логином уже существует");

            var user = new User
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role ?? "Student"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Регистрация прошла успешно");
        }

        [HttpPost("adminOnly")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnlyAction()
        {
            return Ok("Only accessible by Admins!");
        }

        [HttpPost("teacherOnly")]
        [Authorize(Roles = "Teacher")]
        public IActionResult TeacherOnlyAction()
        {
            return Ok("Only accessible by Teachers!");
        }

        [HttpPost("studentOnly")]
        [Authorize(Roles = "Student")]
        public IActionResult StudentOnlyAction()
        {
            return Ok("Only accessible by Students!");
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var username = User.Identity?.Name;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Username = username,
                Role = role
            });
        }

    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Role { get; set; } = "Student";
    }
}
