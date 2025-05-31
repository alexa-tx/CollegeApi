using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace CollegeApi.DTOs
{
    public class ScheduleForm
    {
        [SwaggerSchema("Время начала")]
        [Required(ErrorMessage = "Время начала обязательно")]
        public DateTime StartTime { get; set; }

        [SwaggerSchema("Время окончания")]
        [Required(ErrorMessage = "Время окончания обязательно")]
        public DateTime EndTime { get; set; }

        [SwaggerSchema("ID группы")]
        [Required(ErrorMessage = "ID группы обязателен")]
        public int GroupId { get; set; }

        [SwaggerSchema("ID преподавателя")]
        [Required(ErrorMessage = "ID преподавателя обязателен")]
        public int TeacherProfileId { get; set; }

        [SwaggerSchema("ID предмета")]
        [Required(ErrorMessage = "ID предмета обязателен")]
        public int SubjectId { get; set; }
    }
}
