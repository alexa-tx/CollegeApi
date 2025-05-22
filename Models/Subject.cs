namespace CollegeApi.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ScheduleItem> Lessons { get; set; } = new();
        public List<TeacherSubject> TeacherSubjects { get; set; } = new();
    }
}
