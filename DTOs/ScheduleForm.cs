using System.ComponentModel.DataAnnotations;

namespace CollegeApi.DTOs
{
    public class ScheduleForm
    {
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public int TeacherProfileId { get; set; }

        [Required]
        public int SubjectId { get; set; }
    }
}
