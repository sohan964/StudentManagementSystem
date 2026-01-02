namespace StudentManagementSystem.Models.GradeDtos
{
    public class GradeListsDto
    {
        public int? Grade_id { get; set; }
        public string? Grade_name { get; set; }
        public int? Min_mark { get; set; }
        public int? Max_mark { get; set; }
        public decimal? Grade_point { get; set; }
    }
}
