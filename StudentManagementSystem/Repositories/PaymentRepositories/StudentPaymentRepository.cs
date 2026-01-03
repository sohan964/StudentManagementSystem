using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.PaymentDtos;
using System.Data;
namespace StudentManagementSystem.Repositories.PaymentRepositories
{
    public class StudentPaymentRepository(IConfiguration _configuration) : IStudentPaymentRepository
    {
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

        //student payment
        public async Task<Response<object>> SubmitStudentPaymentAsync(SubmitPaymentDto submitPayment)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spSubmitStudentPayment", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            command.Parameters.AddWithValue("@student_fee_id", submitPayment.Student_fee_id);
            command.Parameters.AddWithValue("@paid_amount", submitPayment.Paid_amount);
            command.Parameters.AddWithValue("@payment_method", submitPayment.Payment_method);
            command.Parameters.AddWithValue("@reference_no", submitPayment.Reference_no);
            await connection.OpenAsync();
            var response = await command.ExecuteScalarAsync();

            return new Response<object>(true, "added success", response);
        }
    }
}
