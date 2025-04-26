namespace CollegeApi.Models
{
    public class HomeworkSubmission
    {
        public int Id { get; set; }
        public int StudentProfileId { get; set; }
        public StudentProfile StudentProfile { get; set; }
        public int HomeworkId { get; set; }
        public Homework Homework { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
        public string Content { get; set; } = string.Empty; // Ссылка или текст
        public int? Grade { get; set; } // Оценка за задание (опционально)
        public string? Comment { get; set; } // Комментарий к сдаче
    }
}
