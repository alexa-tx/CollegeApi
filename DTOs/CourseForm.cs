using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace CollegeApi.DTOs
{
    public class CourseForm
    {
        [FromForm]
        [SwaggerSchema("Название курса")]
        [Required(ErrorMessage = "Введите название курса")]
        public string Title { get; set; } = string.Empty;

        [FromForm]
        [SwaggerSchema("Описание курса")]
        public string Description { get; set; } = string.Empty;

        [FromForm]
        [SwaggerSchema("ID преподавателя, ведущего курс")]
        [Required(ErrorMessage = "Укажите ID преподавателя")]
        public int TeacherProfileId { get; set; }
    }
}
