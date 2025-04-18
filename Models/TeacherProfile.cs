namespace CollegeApi.Models
{
    public class TeacherProfile
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        // Связь с пользователем
        public int UserId { get; set; }
        public User User { get; set; }

        public List<Course> Courses { get; set; } = new();
    }

}
