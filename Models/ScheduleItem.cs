namespace CollegeApi.Models
{
    public class ScheduleItem
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public int TeacherProfileId { get; set; }
        public TeacherProfile? Teacher { get; set; }

        public int SubjectId { get; set; }
        public Subject? Subject { get; set; } // исправь с Subjects
    }


}
