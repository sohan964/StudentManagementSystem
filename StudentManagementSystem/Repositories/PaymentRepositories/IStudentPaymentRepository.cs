using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.PaymentDtos;

namespace StudentManagementSystem.Repositories.PaymentRepositories
{
    public interface IStudentPaymentRepository
    {
        Task<Response<object>> SubmitStudentPaymentAsync(SubmitPaymentDto submitPayment);
        Task<Response<object>> ApproveOrRejectPaymentAsync(int payment_id, string payment_status);
        Task<Response<List<PendingPaymentDto>>> GetPendingPaymentsAsync();
    }
}
