using Swashbuckle.AspNetCore.Annotations;

namespace CollegeApi.DTOs
{
    public class HomeworkSubmissionDto
    {
        [SwaggerSchema("ID отправки")]
        public int Id { get; set; }

        [SwaggerSchema("Имя студента")]
        public string StudentName { get; set; } = string.Empty;

        [SwaggerSchema("Название задания")]
        public string HomeworkTitle { get; set; } = string.Empty;

        [SwaggerSchema("Дата отправки")]
        public DateTime SubmissionDate { get; set; }

        [SwaggerSchema("Содержимое ответа")]
        public string Content { get; set; } = string.Empty;

        [SwaggerSchema("Оценка (если есть)")]
        public int? Grade { get; set; }

        [SwaggerSchema("Комментарий преподавателя")]
        public string? Comment { get; set; }
    }
}
