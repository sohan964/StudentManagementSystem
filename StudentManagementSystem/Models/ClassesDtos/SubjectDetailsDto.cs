namespace StudentManagementSystem.Models.ClassesDtos
{
    public class SubjectDetailsDto
    {
        public int? Subject_id { get; set; }
        public string? Subject_code { get; set; }
        public string? Name { get; set; }
        public bool? Is_theory { get; set; }
        public bool? Is_practical { get; set; }
        public int? Default_marks { get; set; }
        public bool? Is_mandatory { get; set; }
    }
}
