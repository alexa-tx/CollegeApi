using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace CollegeApi.DTOs
{
    public class LoginForm
    {
        [FromForm]
        [SwaggerSchema("Имя пользователя")]
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        public string Username { get; set; } = string.Empty;

        [FromForm]
        [SwaggerSchema("Пароль")]
        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; } = string.Empty;
    }
}
