namespace StudentManagementSystem.Models.ExamDtos
{
    public class GetExamSessionDto
    {
        public int? Exam_session_id { get; set; }
        public int? Year_id { get; set; }
        public int? Exam_type_id { get; set; }
        public string? Exam_type_name { get; set; }
        public int? Subject_id { get; set; }
        public string? Subject_name { get; set;}
        public int? Class_id { get; set; }
        public int? Section_id { get; set; }
        public DateOnly? Exam_date {  get; set; }
        public Decimal? Max_marks { get; set; }
        public int? Exam_slot_id { get; set; }
    }
}
