// DTOs/TeacherSubjectForm.cs
using Microsoft.AspNetCore.Mvc;

namespace CollegeApi.DTOs
{
    public class TeacherSubjectForm
    {
        [FromForm]
        public int TeacherProfileId { get; set; }

        [FromForm]
        public int SubjectId { get; set; }
    }
}
