namespace StudentManagementSystem.Models.ClassesDtos
{
    public class ClassSubjectsDetailsDto
    {
        public int Class_id { get; set; }
        public string? Class_name { get; set;}
        public string? Class_description { get; set; }
        public List<SubjectDetailsDto> SubjectList { get; set; } = new();
    }
}
