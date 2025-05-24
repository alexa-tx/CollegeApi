using Microsoft.AspNetCore.Mvc;

namespace CollegeApi.DTOs
{
    public class RegisterForm
    {
        [FromForm]
        public string Username { get; set; } = string.Empty;

        [FromForm]
        public string Password { get; set; } = string.Empty;

        [FromForm]
        public string? Role { get; set; }

        [FromForm(Name = "lastName")]
        public string LastName { get; set; } = string.Empty;

        [FromForm(Name = "firstName")]
        public string FirstName { get; set; } = string.Empty;

        [FromForm(Name = "middleName")]
        public string MiddleName { get; set; } = string.Empty;
    }
}
