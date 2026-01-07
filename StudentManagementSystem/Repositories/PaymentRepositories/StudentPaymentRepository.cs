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

        //admin accept payment
        public async Task<Response<object>> ApproveOrRejectPaymentAsync(int payment_id, string payment_status)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spApproveOrRejectPayment", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@payment_id", payment_id);
            command.Parameters.AddWithValue("@payment_status", payment_status);
            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            if (result == 0) return new Response<object>(false, "Server problem try again");
            return new Response<object>(true, $"successfully {payment_status}");
        }

        public async Task<Response<List<PendingPaymentDto>>> GetPendingPaymentsAsync()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetPendingPaymentsForAdmin", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var pendingPaymentList = new List<PendingPaymentDto>();
            while( await reader.ReadAsync() )
            {
                pendingPaymentList.Add(new PendingPaymentDto()
                {
                    Payment_id = reader.GetInt32(0),
                    Student_fee_id = reader.GetInt32(1),
                    Student_id = reader.GetInt32(2),
                    Student_number = reader.GetString(3),
                    Student_name = reader.GetString(4),
                    Enrollment_id = reader.GetInt32(5),
                    Class_name = reader.GetString(6),
                    Section_name = reader.GetString(7),
                    Year_label = reader.GetString(8),
                    Month_no = reader.GetInt32(9),
                    Month_name = reader.GetString(10),
                    Fee_amount = reader.GetDecimal(11),
                    Paid_amount = reader.GetDecimal(12),
                    Payment_method = reader.GetString(13),
                    Reference_no = reader.GetString(14),
                    Payment_date = reader.GetDateTime(15),
                    Payment_status = reader.GetString(16),
                });
            }

            if (pendingPaymentList.Count == 0) return new Response<List<PendingPaymentDto>>(false, "No Pending Paymensts");
            return new Response<List<PendingPaymentDto>>(true, "All Pending Payments list", pendingPaymentList);
        }
    }
}
