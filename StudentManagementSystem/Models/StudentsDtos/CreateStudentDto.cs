namespace StudentManagementSystem.Models.StudentsDtos
{
    public class CreateStudentDto
    {
        public string? User_id { get; set; }
        public string? First_name { get; set; }
        public string? Last_name { get; set;}
        public DateOnly? DOB {  get; set; }
        public char? Gender { get; set; }
        public string? Photo {  get; set; }
        public int? Admission_year { get; set; }
        public string? Address { get; set; }
    }
}
