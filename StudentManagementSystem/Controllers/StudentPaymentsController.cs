using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.PaymentDtos;
using StudentManagementSystem.Repositories.PaymentRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentPaymentsController : ControllerBase
    {
        private readonly IStudentPaymentRepository _paymentRepository;

        public StudentPaymentsController(IStudentPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [HttpPost("submit-payment")]
        public async Task<IActionResult> SubmitStudentPayment([FromBody] SubmitPaymentDto submitPaymentDto)
        {
            var result = await _paymentRepository.SubmitStudentPaymentAsync(submitPaymentDto);
            return Ok(result);
        }

        [HttpPut("payment-status-update")]
        public async Task<IActionResult> ApproveOrRejectPayment([FromQuery]int payment_id, [FromQuery] string payment_status)
        {
            var result = await _paymentRepository.ApproveOrRejectPaymentAsync(payment_id, payment_status);
            return Ok(result);
        }

        [HttpGet("pending-payments")]
        public async Task<IActionResult> GetPendingPayments()
        {
            var result = await _paymentRepository.GetPendingPaymentsAsync();
            
            return Ok(result);
        }
    }
}
