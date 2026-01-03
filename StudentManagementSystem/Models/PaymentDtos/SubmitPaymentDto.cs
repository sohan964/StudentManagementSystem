namespace StudentManagementSystem.Models.PaymentDtos
{
    public class SubmitPaymentDto
    {
        public int Student_fee_id { get; set; }
        public decimal Paid_amount { get; set; }
        public string Payment_method { get; set; }
        public string Reference_no { get; set; }
    }
}
