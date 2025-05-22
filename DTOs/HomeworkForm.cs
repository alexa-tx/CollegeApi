using Microsoft.AspNetCore.Mvc;

namespace CollegeApi.DTOs
{
    public class HomeworkForm
    {
        [FromForm]
        public string Title { get; set; } = string.Empty;

        [FromForm]
        public string Description { get; set; } = string.Empty;

        [FromForm]
        public DateTime DueDate { get; set; }

        [FromForm]
        public int SubjectId { get; set; }
    }
}
