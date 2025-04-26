namespace CollegeApi.Models
{
    public class Homework
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; } = new Course();  // Убедитесь, что объект Course передается корректно
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
