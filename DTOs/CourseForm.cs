using Microsoft.AspNetCore.Mvc;

namespace CollegeApi.DTOs
{
    public class CourseForm
    {
        [FromForm]
        public string Title { get; set; } = string.Empty;

        [FromForm]
        public string Description { get; set; } = string.Empty;

        [FromForm]
        public int TeacherProfileId { get; set; }
    }
}
