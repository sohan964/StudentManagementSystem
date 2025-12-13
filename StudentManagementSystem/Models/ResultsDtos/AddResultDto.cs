namespace StudentManagementSystem.Models.ResultsDtos
{
    public class AddResultDto
    {
        public int? Exam_session_id { get; set; }
        public int? Enrollment_id { get; set; }
        public Decimal? Obtained_marks { get; set; }
    }
}
