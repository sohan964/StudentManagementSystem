namespace StudentManagementSystem.Models.StudentMonthlyFeeDtos
{
    public class GenerateMonthlyFeesDto
    {
        public int? Year_id { get; set; }
        public int? Fee_month_id { get; set; }
        public DateOnly? Due_date { get; set; }
    }
}
