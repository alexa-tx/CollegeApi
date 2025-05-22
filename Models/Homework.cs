namespace CollegeApi.Models
{
    public class Homework
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }

        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
