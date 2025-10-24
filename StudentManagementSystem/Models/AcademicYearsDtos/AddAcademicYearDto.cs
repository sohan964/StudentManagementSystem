namespace StudentManagementSystem.Models.AcademicYearsDtos
{
    public class AddAcademicYearDto
    {
        public string? Year_lable { get; set; }
        public DateOnly? Start_date { get; set; }
        public DateOnly? End_date { get; set;}
        public bool? Is_active { get; set; }
    }
}
