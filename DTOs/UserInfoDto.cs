using Swashbuckle.AspNetCore.Annotations;

namespace CollegeApi.DTOs
{
    public class UserInfoDto
    {
        [SwaggerSchema("ID пользователя")]
        public int Id { get; set; }

        [SwaggerSchema("Имя пользователя")]
        public string Username { get; set; } = string.Empty;

        [SwaggerSchema("Роль")]
        public string Role { get; set; } = string.Empty;

        [SwaggerSchema("Полное имя (если указано)")]
        public string? FullName { get; set; }

        [SwaggerSchema("Тип профиля (если применимо)")]
        public string? ProfileType { get; set; }
    }
}
