namespace CollegeApi.DTOs
{
    public class HomeworkSubmissionDto
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string HomeworkTitle { get; set; } = string.Empty;
        public DateTime SubmissionDate { get; set; }
        public string Content { get; set; } = string.Empty;
        public int? Grade { get; set; }
        public string? Comment { get; set; }
    }
}
