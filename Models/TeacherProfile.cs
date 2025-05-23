namespace CollegeApi.Models
{
    public class TeacherProfile
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User User { get; set; }

        public List<Course> Courses { get; set; } = new();
        public List<TeacherSubject> TeacherSubjects { get; set; } = new();
        public string? Bio { get; set; }
        public string? TelegramLink { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? AvatarUrl { get; set; } // путь к загруженному изображению

    }


}
