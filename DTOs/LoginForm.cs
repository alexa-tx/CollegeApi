using Microsoft.AspNetCore.Mvc;

namespace CollegeApi.DTOs
{
    public class LoginForm
    {
        [FromForm]
        public string Username { get; set; } = string.Empty;

        [FromForm]
        public string Password { get; set; } = string.Empty;
    }
}
