namespace StudentManagementSystem.Models.ExamDtos
{
    public class CreateExamSessionDto
    {
        public int? Year_id { get; set; }
        public int? Exam_type_id { get; set; }
        public int? Subject_id { get; set; }
        public int? Class_id { get; set; }
        public int? Section_id { get; set; }
        public DateOnly? Exam_date { get; set; }
        public int? Exam_slot_id { get; set; }
        public Decimal? Max_marks { get; set; }

    }
}
