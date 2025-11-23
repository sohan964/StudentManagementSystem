namespace StudentManagementSystem.Models.SubjectDtos
{
    public class SubjectsDto
    {
        public int? Subject_id { get; set; }
        public string? Subject_code { get; set; }
        public string? Name { get; set; }
        public bool? Is_theory { get; set; }
        public bool? Is_practical { get; set; }
        public int? Default_marks { get; set; }
        public int? Department_id { get; set; }
        public int? Credit_hours { get; set; }
    }
}
