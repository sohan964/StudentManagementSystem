namespace StudentManagementSystem.Models.ResultsDtos
{
    public class StudentSubjectResultDto
    {
        public int? Result_id { get; set; }
        public int? Exam_session_id { get; set; }
        public int? Enrollment_id { get; set; }
        public Decimal? obtained_marks { get; set; }
        
    }
}
