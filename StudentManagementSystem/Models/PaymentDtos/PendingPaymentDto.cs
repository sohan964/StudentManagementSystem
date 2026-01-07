namespace StudentManagementSystem.Models.PaymentDtos
{
    public class PendingPaymentDto
    {
        public int Payment_id { get; set; }
        public int Student_fee_id { get; set; }
        public int Student_id { get; set; }
        public string Student_number { get; set; }
        public string Student_name { get; set; }
        public int Enrollment_id { get; set; }
        public string Class_name { get; set; }
        public string Section_name { get; set; }
        public string Year_label { get; set; }
        public int Month_no { get; set; }
        public string Month_name { get; set; }
        public decimal Fee_amount { get; set; }
        public decimal Paid_amount { get; set; }
        public string Payment_method {  get; set; }
        public string Reference_no { get; set; }
        public DateTime? Payment_date { get; set; }
        public string Payment_status { get; set; }
    }
}
