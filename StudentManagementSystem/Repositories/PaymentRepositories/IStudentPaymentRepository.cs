using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.PaymentDtos;

namespace StudentManagementSystem.Repositories.PaymentRepositories
{
    public interface IStudentPaymentRepository
    {
        Task<Response<object>> SubmitStudentPaymentAsync(SubmitPaymentDto submitPayment);
    }
}
