using Microsoft.AspNetCore.Mvc;

namespace CollegeApi.DTOs
{
    public class NewsPostForm
    {
        [FromForm]
        public string Title { get; set; } = string.Empty;

        [FromForm]
        public string Content { get; set; } = string.Empty;

        [FromForm]
        public IFormFile? Image { get; set; }
    }

}
