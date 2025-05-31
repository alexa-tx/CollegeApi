using Swashbuckle.AspNetCore.Annotations;

namespace CollegeApi.DTOs
{
    public class UserProfileDto
    {
        [SwaggerSchema("Полное имя")]
        public string? FullName { get; set; }

        [SwaggerSchema("Биография")]
        public string? Bio { get; set; }

        [SwaggerSchema("Ссылка на Telegram")]
        public string? TelegramLink { get; set; }

        [SwaggerSchema("Дата рождения")]
        public DateTime? BirthDate { get; set; }

        [SwaggerSchema("URL аватара")]
        public string? AvatarUrl { get; set; }

        [SwaggerSchema("Роль пользователя")]
        public string Role { get; set; } = string.Empty;

        [SwaggerSchema("Имя пользователя")]
        public string Username { get; set; } = string.Empty;
    }
}
