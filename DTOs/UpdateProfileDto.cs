using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace CollegeApi.DTOs
{
    public class UpdateProfileDto
    {
        [SwaggerSchema("Полное имя (опционально)")]
        public string? FullName { get; set; }

        [SwaggerSchema("Биография (опционально)")]
        public string? Bio { get; set; }

        [SwaggerSchema("Ссылка на Telegram (опционально)")]
        public string? TelegramLink { get; set; }

        [SwaggerSchema("Дата рождения (опционально)")]
        public DateTime? BirthDate { get; set; }

        [SwaggerSchema("Аватар (изображение, опционально)")]
        public IFormFile? Avatar { get; set; }
    }
}
