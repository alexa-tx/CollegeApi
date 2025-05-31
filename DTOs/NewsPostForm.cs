using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace CollegeApi.DTOs
{
    public class NewsPostForm
    {
        [FromForm]
        [SwaggerSchema("Заголовок новости")]
        [Required(ErrorMessage = "Заголовок обязателен")]
        public string Title { get; set; } = string.Empty;

        [FromForm]
        [SwaggerSchema("Содержимое новости")]
        [Required(ErrorMessage = "Содержимое обязательно")]
        public string Content { get; set; } = string.Empty;

        [FromForm]
        [SwaggerSchema("Изображение (опционально)")]
        public IFormFile? Image { get; set; }
    }
}
