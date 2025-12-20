namespace StudentManagementSystem.Models.ResultsDtos
{
    public class OverAllResultDto
    {
        public int? Enrollment_id { get; set; }
        public string? Student_number { get; set; }
        public string? Student_name { get; set; }
        public string? Class_name { get; set; }
        public string? Section_name { get; set; }
        public string? Year_label { get; set; }
        public int? Total_credit_hours { get; set; }
        public decimal? Total_weighted_grade_points { get; set; }
        public decimal? Gpa { get; set; }
        public string? Overall_grade {  get; set; }
        public List<EachSubjectResultDto> EachSubjectResultDtos { get; set; }
    }

    public class EachSubjectResultDto
    {
        public int? Subject_id { get; set; }
        public string? Subject_name { get; set; }
        public string? Subject_code { get; set; }
        public int? Credit_hours { get; set; }
        public decimal? Total_marks { get; set; }
        public decimal? Max_marks { get; set; }
        public decimal? Percentage { get; set; }
        public string? Grade_name { get; set; }
        public decimal? Grade_point { get; set; }
        public decimal? Weighted_grade_point { get; set; }
    }
}
