using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace CollegeApi.DTOs
{
    public class GroupForm
    {
        [FromForm]
        [SwaggerSchema("Название группы")]
        [Required(ErrorMessage = "Укажите название группы")]
        public string Name { get; set; } = string.Empty;

        [FromForm]
        [SwaggerSchema("Массив ID студентов, входящих в группу")]
        public int[] StudentProfileIds { get; set; } = Array.Empty<int>();
    }
}
