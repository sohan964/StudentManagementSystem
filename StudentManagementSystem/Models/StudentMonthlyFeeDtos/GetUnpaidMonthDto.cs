namespace StudentManagementSystem.Models.StudentMonthlyFeeDtos
{
    public class GetUnpaidMonthDto
    {
        public int? Student_fee_id {  get; set; }
        public int? Enrollment_id { get; set; }
        public int? Year_id { get; set; }
        public string? Year_label { get; set; }
        public int? Month_no { get; set; }
        public string? Month_name { get; set; }
        public decimal? Fee_amount { get; set; }
        public decimal? Paid_amount { get; set; }
        public decimal? Due_amount { get; set; }
        public DateOnly? Due_date { get; set; }
        public string? Payment_status { get; set; }
    }
}
