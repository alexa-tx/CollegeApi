namespace CollegeApi.Models
{
    public class ScheduleItem
    {
        public int Id { get; set; }

        public string Subject { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int TeacherProfileId { get; set; }
        public TeacherProfile Teacher { get; set; }
        public Course Course { get; set; }
    }

}
