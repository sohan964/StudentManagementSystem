namespace StudentManagementSystem.Models.EnrollmentsDtos
{
    public class EnrollmentsDto
    {
        public int? Enrollment_id { get; set; }
        public int? Student_id { get; set; }
        public string? Student_name { get; set; }
        public string? Student_number { get; set; }
        public int? Year_id { get; set; }
        public int? Class_id { get; set; }
        public int? Section_id { get; set; }
        public string? Status { get; set; }

    }
}
