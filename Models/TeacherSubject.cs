namespace CollegeApi.Models
{
    public class TeacherSubject
    {
        public int TeacherProfileId { get; set; }
        public TeacherProfile TeacherProfile { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }

}
