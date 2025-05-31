using CollegeApi.Data;
using CollegeApi.DTOs;
using CollegeApi.Interfaces;
using CollegeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace CollegeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Управление авторизацией и пользователями")]
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
        [SwaggerOperation(
             Summary = "Вход в систему",
             Description = "Позволяет пользователю войти в систему, получив JWT-токен"
         )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [SwaggerOperation(
            Summary = "Регистрация нового пользователя",
            Description = "Создаёт пользователя и профиль (студента или преподавателя)"
        )]
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
            else 
            {
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
        [SwaggerOperation(Summary = "Только для администратора")]
        public IActionResult AdminOnlyAction() => Ok("Только для администратора");

        [HttpPost("teacherOnly")]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(Summary = "Только для преподавателя")]
        public IActionResult TeacherOnlyAction() => Ok("Только для преподавателя");

        [HttpPost("studentOnly")]
        [Authorize(Roles = "Student")]
        [SwaggerOperation(Summary = "Только для студента")]
        public IActionResult StudentOnlyAction() => Ok("Только для студента");

        [HttpGet("me")]
        [Authorize]
        [SwaggerOperation(Summary = "Информация о текущем пользователе")]
        public IActionResult Me()
        {
            var username = User.Identity?.Name;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return Ok(new { Username = username, Role = role });
        }

        [HttpPut("update/{userId}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Обновить пользователя", Description = "Доступно только администратору")]
        public async Task<IActionResult> UpdateUser(int userId, [FromForm] UpdateUserForm form)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("Пользователь не найден");

            if (!string.IsNullOrEmpty(form.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(form.Password);
            }

            if (!string.IsNullOrEmpty(form.Role))
            {
                user.Role = form.Role;
            }

            if (!string.IsNullOrEmpty(form.FullName))
            {
                if (user.Role == "Teacher")
                {
                    var teacherProfile = await _context.TeacherProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
                    if (teacherProfile != null) teacherProfile.FullName = form.FullName;
                }
                else if (user.Role == "Student")
                {
                    var studentProfile = await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
                    if (studentProfile != null) studentProfile.FullName = form.FullName;
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Пользователь обновлен");
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Удалить пользователя", Description = "Удаляет пользователя и его профиль")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("Пользователь не найден");

            if (user.Role == "Teacher")
            {
                var teacherProfile = await _context.TeacherProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
                if (teacherProfile != null) _context.TeacherProfiles.Remove(teacherProfile);
            }
            else if (user.Role == "Student")
            {
                var studentProfile = await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
                if (studentProfile != null) _context.StudentProfiles.Remove(studentProfile);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("Пользователь удален");
        }
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Список всех пользователей")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            var userInfos = new List<UserInfoDto>();

            foreach (var user in users)
            {
                string? fullName = null;
                string? profileType = null;

                if (user.Role == "Teacher")
                {
                    var teacher = await _context.TeacherProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
                    if (teacher != null)
                    {
                        fullName = teacher.FullName;
                        profileType = "Teacher";
                    }
                }
                else if (user.Role == "Student")
                {
                    var student = await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
                    if (student != null)
                    {
                        fullName = student.FullName;
                        profileType = "Student";
                    }
                }

                userInfos.Add(new UserInfoDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Role = user.Role,
                    FullName = fullName,
                    ProfileType = profileType
                });
            }

            return Ok(userInfos);
        }
        [HttpPut("profile")]
        [Authorize]
        [SwaggerOperation(Summary = "Обновить собственный профиль")]
        public async Task<IActionResult> UpdateOwnProfile([FromForm] UpdateProfileDto dto)
        {
            var username = User.Identity?.Name;
            if (username == null) return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return NotFound("Пользователь не найден");

            string? filePath = null;

            // аватарка
            if (dto.Avatar != null)
            {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");
                Directory.CreateDirectory(uploadsDir);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Avatar.FileName)}";
                filePath = Path.Combine(uploadsDir, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.Avatar.CopyToAsync(stream);
            }

            if (user.Role == "Teacher")
            {
                var profile = await _context.TeacherProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
                if (profile == null) return NotFound("Профиль не найден");

                if (!string.IsNullOrEmpty(dto.FullName)) profile.FullName = dto.FullName;
                if (!string.IsNullOrEmpty(dto.Bio)) profile.Bio = dto.Bio;
                if (!string.IsNullOrEmpty(dto.TelegramLink)) profile.TelegramLink = dto.TelegramLink;
                if (dto.BirthDate != null) profile.BirthDate = dto.BirthDate;
                if (filePath != null) profile.AvatarUrl = $"/avatars/{Path.GetFileName(filePath)}";
            }
            else if (user.Role == "Student")
            {
                var profile = await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
                if (profile == null) return NotFound("Профиль не найден");

                if (!string.IsNullOrEmpty(dto.FullName)) profile.FullName = dto.FullName;
                if (!string.IsNullOrEmpty(dto.Bio)) profile.Bio = dto.Bio;
                if (!string.IsNullOrEmpty(dto.TelegramLink)) profile.TelegramLink = dto.TelegramLink;
                if (dto.BirthDate != null) profile.BirthDate = dto.BirthDate;
                if (filePath != null) profile.AvatarUrl = $"/avatars/{Path.GetFileName(filePath)}";
            }

            await _context.SaveChangesAsync();
            return Ok("Профиль обновлен");
        }
        [HttpGet("profile")]
        [Authorize]
        [SwaggerOperation(Summary = "Получить собственный профиль")]
        public async Task<IActionResult> GetOwnProfile()
        {
            var username = User.Identity?.Name;
            if (username == null) return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return NotFound("Пользователь не найден");

            var profileDto = new UserProfileDto
            {
                Username = user.Username,
                Role = user.Role
            };

            if (user.Role == "Teacher")
            {
                var profile = await _context.TeacherProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
                if (profile != null)
                {
                    profileDto.FullName = profile.FullName;
                    profileDto.Bio = profile.Bio;
                    profileDto.TelegramLink = profile.TelegramLink;
                    profileDto.BirthDate = profile.BirthDate;
                    profileDto.AvatarUrl = profile.AvatarUrl;
                }
            }
            else if (user.Role == "Student")
            {
                var profile = await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
                if (profile != null)
                {
                    profileDto.FullName = profile.FullName;
                    profileDto.Bio = profile.Bio;
                    profileDto.TelegramLink = profile.TelegramLink;
                    profileDto.BirthDate = profile.BirthDate;
                    profileDto.AvatarUrl = profile.AvatarUrl;
                }
            }

            return Ok(profileDto);
        }
        [HttpPost("add-user")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Добавить пользователя (админ)", Description = "Добавление нового студента или преподавателя вручную")]
        public async Task<IActionResult> AddUser([FromForm] AdminAddUserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest("Пользователь с таким логином уже существует");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var fullName = $"{dto.LastName} {dto.FirstName} {dto.MiddleName}".Trim();

            if (dto.Role == "Teacher")
            {
                var teacherProfile = new TeacherProfile
                {
                    FullName = fullName,
                    UserId = user.Id
                };
                _context.TeacherProfiles.Add(teacherProfile);
            }
            else
            {
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
            return Ok("Пользователь успешно добавлен");
        }

    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
