using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeApi.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TeacherProfileId { get; set; }

        [ForeignKey("TeacherProfileId")]
        public TeacherProfile? Teacher { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new();

    }

}
