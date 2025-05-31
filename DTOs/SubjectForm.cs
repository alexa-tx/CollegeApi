using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace CollegeApi.DTOs
{
    public class SubjectForm
    {
        [FromForm]
        [SwaggerSchema("Название предмета")]
        [Required(ErrorMessage = "Название предмета обязательно")]
        public string Name { get; set; } = string.Empty;
    }
}
