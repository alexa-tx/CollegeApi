using System.Text.Json.Serialization;

namespace CollegeApi.Models
{
    public enum GradeType
    {
        Exam = 0,
        Test = 1,
        Homework = 2,
        Project = 3,
        Other = 4
    }
    public class Grade
    {
        public int Id { get; set; }

        public int StudentProfileId { get; set; }

        [JsonIgnore]
        public StudentProfile? StudentProfile { get; set; }

        public int CourseId { get; set; }

        [JsonIgnore]
        public Course? Course { get; set; }

        public int Value { get; set; }

        public string Comment { get; set; } = string.Empty;
        public GradeType GradeType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
