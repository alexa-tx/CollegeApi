using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace CollegeApi.DTOs
{
    public class AdminAddUserDto
    {
        [SwaggerSchema("Имя пользователя (логин)")]
        [Required(ErrorMessage = "Введите имя пользователя")]
        public string Username { get; set; } = string.Empty;

        [SwaggerSchema("Пароль")]
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; } = string.Empty;

        [SwaggerSchema("Роль (например: Student, Teacher, Admin)")]
        public string Role { get; set; } = "Student";

        [SwaggerSchema("Имя")]
        [Required(ErrorMessage = "Введите имя")]
        public string FirstName { get; set; } = string.Empty;

        [SwaggerSchema("Фамилия")]
        [Required(ErrorMessage = "Введите фамилию")]
        public string LastName { get; set; } = string.Empty;

        [SwaggerSchema("Отчество (опционально)")]
        public string? MiddleName { get; set; }
    }
}
