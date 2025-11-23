namespace StudentManagementSystem.Models.EnrollmentsDtos
{
    public class AddEnrollmentDto
    {
        public int? Student_id { get; set; }
        public int? Year_id { get; set; }
        public int? Class_id { get; set; }
        public int? Section_id { get; set; }
        public DateOnly? Admission_date { get; set; }
        public string? Status { get; set; }
    }
}
