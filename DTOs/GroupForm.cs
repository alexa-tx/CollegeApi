using Microsoft.AspNetCore.Mvc;

namespace CollegeApi.DTOs
{
    public class GroupForm
    {
        [FromForm]
        public string Name { get; set; } = string.Empty;

        // Массив ID студентов, которых нужно добавить в группу
        [FromForm]
        public int[] StudentProfileIds { get; set; } = Array.Empty<int>();
    }
}
