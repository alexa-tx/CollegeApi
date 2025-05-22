namespace CollegeApi.Models
{
    public class NewsPost
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
        public string? ImageUrl { get; set; }
    }

}
