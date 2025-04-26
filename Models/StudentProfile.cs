using CollegeApi.Models;

public class StudentProfile
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User? User { get; set; }

    public int GroupId { get; set; }
    public Group? Group { get; set; }

    public List<Enrollment> Enrollments { get; set; } = new();
}
