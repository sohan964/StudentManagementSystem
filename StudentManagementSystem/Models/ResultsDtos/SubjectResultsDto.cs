namespace StudentManagementSystem.Models.ResultsDtos
{
    public class SubjectResultsDto
    {
        public int? Subject_id { get; set; } = null;
        public string? Subject_name { get; set; } = null;
        public int? Exam_type_id { get; set; } = null;
        public string? Type_name { get; set; } = null;
        public decimal? Marks { get; set; } = null;
        public decimal? Weight_percentage { get; set; } = null;
    }
}
