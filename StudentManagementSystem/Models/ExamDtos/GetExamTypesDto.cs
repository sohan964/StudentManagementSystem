namespace StudentManagementSystem.Models.ExamDtos
{
    public class GetExamTypesDto
    {
        public int? Exam_type_id { get; set; }
        public string? Type_name { get; set; }
        public decimal? Weight_percentage { get; set; }
    }
}
