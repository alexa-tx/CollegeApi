using Microsoft.AspNetCore.Mvc;

namespace CollegeApi.DTOs
{
    public class SubjectForm
    {
        [FromForm]
        public string Name { get; set; } = string.Empty;
    }
}
