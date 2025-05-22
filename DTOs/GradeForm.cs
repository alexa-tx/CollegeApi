using CollegeApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApi.DTOs
{
    public class GradeForm
    {
        [FromForm]
        public int StudentProfileId { get; set; }

        [FromForm]
        public int SubjectId { get; set; }

        [FromForm]
        public int Value { get; set; }

        [FromForm]
        public string Comment { get; set; } = string.Empty;

        [FromForm]
        public GradeType GradeType { get; set; }
    }
}
