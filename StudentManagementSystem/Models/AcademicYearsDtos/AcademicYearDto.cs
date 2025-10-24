namespace StudentManagementSystem.Models.AcademicYearsDtos
{
    public class AcademicYearDto
    {
        public int? Year_id { get; set; }
        public string? Year_lable { get; set; }
        public DateOnly? Start_date { get; set; }
        public DateOnly? End_date { get; set; }
        public bool? Is_active { get; set; }
    }
}
