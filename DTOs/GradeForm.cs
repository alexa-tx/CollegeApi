using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using CollegeApi.Models;

namespace CollegeApi.DTOs
{
    public class GradeForm
    {
        [FromForm]
        [SwaggerSchema("ID студента")]
        [Required(ErrorMessage = "Укажите ID студента")]
        public int StudentProfileId { get; set; }

        [FromForm]
        [SwaggerSchema("ID предмета")]
        [Required(ErrorMessage = "Укажите ID предмета")]
        public int SubjectId { get; set; }

        [FromForm]
        [SwaggerSchema("Оценка (значение)")]
        [Range(1, 100, ErrorMessage = "Значение должно быть от 1 до 100")]
        public int Value { get; set; }

        [FromForm]
        [SwaggerSchema("Комментарий к оценке (необязательно)")]
        public string Comment { get; set; } = string.Empty;

        [FromForm]
        [SwaggerSchema("Тип оценки (например: Зачёт, Экзамен и т.п.)")]
        [Required(ErrorMessage = "Укажите тип оценки")]
        public GradeType GradeType { get; set; }
    }
}
