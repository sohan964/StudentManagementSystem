namespace StudentManagementSystem.Models.PaymentDtos
{
    public class GetStudentPaymentDto
    {
        public int? student_fee_id {  get; set; }
        public int? enrollment_id { get; set; }
        public decimal? fee_amount { get; set; }
        public DateOnly? due_date { get; set; }
        public string? fee_status { get; set; }
        public int? fee_month_id { get; set; }
        public string? month_name { get; set; }
        public int? payment_id { get; set; }
        public decimal? paid_amount { get; set; }
        public DateTime? payment_date { get; set; }
        public string? payment_method { get; set; }
        public string? reference_no { get; set; }
        public string? payment_status { get; set; }
    }
}
