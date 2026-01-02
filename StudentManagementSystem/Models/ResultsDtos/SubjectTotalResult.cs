namespace StudentManagementSystem.Models.ResultsDtos
{
    public class SubjectTotalResult
    {
        public int? Subject_id { get; set; }
        public string? Subject_name { get; set; }
        public decimal? Total_marks { get; set; }
        public string? Grade_name { get; set; }
        public decimal? Grade_point { get; set; }

    }
}
