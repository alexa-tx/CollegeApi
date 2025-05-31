using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace CollegeApi.DTOs
{
    public class RegisterForm
    {
        [FromForm]
        [SwaggerSchema("Имя пользователя")]
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        public string Username { get; set; } = string.Empty;

        [FromForm]
        [SwaggerSchema("Пароль")]
        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; } = string.Empty;

        [FromForm]
        [SwaggerSchema("Роль (по умолчанию 'Student')")]
        public string? Role { get; set; }

        [FromForm(Name = "lastName")]
        [SwaggerSchema("Фамилия")]
        [Required(ErrorMessage = "Фамилия обязательна")]
        public string LastName { get; set; } = string.Empty;

        [FromForm(Name = "firstName")]
        [SwaggerSchema("Имя")]
        [Required(ErrorMessage = "Имя обязательно")]
        public string FirstName { get; set; } = string.Empty;

        [FromForm(Name = "middleName")]
        [SwaggerSchema("Отчество (опционально)")]
        public string MiddleName { get; set; } = string.Empty;
    }
}
