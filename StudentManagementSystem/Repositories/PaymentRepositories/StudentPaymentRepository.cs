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

        public async Task<Response<List<GetStudentPaymentDto>>> GetStudentPaymentsByEnrollmentAsync(int? enrollmentId)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetStudentPaymentsByEnrollment", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@enrollment_id", enrollmentId);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var studentPayments = new List<GetStudentPaymentDto>();
            while(await reader.ReadAsync())
            {
                studentPayments.Add(new GetStudentPaymentDto()
                {
                    student_fee_id = reader.GetInt32(0),
                    enrollment_id = reader.GetInt32(1),

                    fee_amount = reader.GetDecimal(2),

                    due_date = reader.IsDBNull(3)
                ? null
                : DateOnly.FromDateTime(reader.GetDateTime(3)),

                    fee_status = reader.IsDBNull(4) ? null : reader.GetString(4),

                    fee_month_id = reader.IsDBNull(5) ? null : reader.GetInt32(5),

                    month_name = reader.IsDBNull(6) ? null : reader.GetString(6),

                    payment_id = reader.IsDBNull(7) ? null : reader.GetInt32(7),

                    paid_amount = reader.IsDBNull(8) ? null : reader.GetDecimal(8),

                    payment_date = reader.IsDBNull(9)
                ? null
                : reader.GetDateTime(9),

                    payment_method = reader.IsDBNull(10) ? null : reader.GetString(10),

                    reference_no = reader.IsDBNull(11) ? null : reader.GetString(11),

                    payment_status = reader.IsDBNull(12) ? null : reader.GetString(12),
                });
            }

            if (studentPayments.Count == 0)
                return new Response<List<GetStudentPaymentDto>>(false, "No payments found");

            return new Response<List<GetStudentPaymentDto>>(true, "Student payments retrieved", studentPayments);


        }
    }
}
