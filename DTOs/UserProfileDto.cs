namespace CollegeApi.DTOs
{
    public class UserProfileDto
    {
        public string? FullName { get; set; }
        public string? Bio { get; set; }
        public string? TelegramLink { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? AvatarUrl { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
