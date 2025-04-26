using CollegeApi.Models;

public class Enrollment
{
    public int Id { get; set; }

    public int StudentProfileId { get; set; }
    public StudentProfile? StudentProfile { get; set; }

    public int CourseId { get; set; }
    public Course? Course { get; set; }

    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
}
