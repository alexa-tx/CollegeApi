using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace CollegeApi.DTOs
{
    public class HomeworkForm
    {
        [FromForm]
        [SwaggerSchema("Заголовок домашнего задания")]
        [Required(ErrorMessage = "Укажите заголовок")]
        public string Title { get; set; } = string.Empty;

        [FromForm]
        [SwaggerSchema("Описание задания")]
        public string Description { get; set; } = string.Empty;

        [FromForm]
        [SwaggerSchema("Дата сдачи")]
        [Required(ErrorMessage = "Укажите срок сдачи")]
        public DateTime DueDate { get; set; }

        [FromForm]
        [SwaggerSchema("ID предмета")]
        [Required(ErrorMessage = "Укажите ID предмета")]
        public int SubjectId { get; set; }
    }
}
