namespace CollegeApi.DTOs
{
    public class UpdateProfileDto
    {
        public string? FullName { get; set; }
        public string? Bio { get; set; }
        public string? TelegramLink { get; set; }
        public DateTime? BirthDate { get; set; }
        public IFormFile? Avatar { get; set; }
    }
}
