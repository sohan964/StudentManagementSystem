namespace StudentManagementSystem.Models.TeachersDtos
{
    public class UpdateTeacherDto
    {
        public string teacher_code { get; set; } = string.Empty;
        public string first_name { get; set; } = string.Empty;
        public string last_name { get; set; } = string.Empty;
        public string contact { get; set; } = string.Empty;
        public string? photo { get; set; }
        public string? description { get; set; }
    }
}
