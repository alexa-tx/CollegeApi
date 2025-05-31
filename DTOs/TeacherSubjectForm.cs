using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace CollegeApi.DTOs
{
    public class TeacherSubjectForm
    {
        [FromForm]
        [SwaggerSchema("ID профиля преподавателя")]
        [Required(ErrorMessage = "ID преподавателя обязателен")]
        public int TeacherProfileId { get; set; }

        [FromForm]
        [SwaggerSchema("ID предмета")]
        [Required(ErrorMessage = "ID предмета обязателен")]
        public int SubjectId { get; set; }
    }
}
