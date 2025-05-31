using Swashbuckle.AspNetCore.Annotations;

namespace CollegeApi.DTOs
{
    public class UpdateUserForm
    {
        [SwaggerSchema("Полное имя (опционально)")]
        public string? FullName { get; set; }

        [SwaggerSchema("Новый пароль (опционально)")]
        public string? Password { get; set; }

        [SwaggerSchema("Роль пользователя (опционально)")]
        public string? Role { get; set; }
    }
}
