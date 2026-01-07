using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.StudentMonthlyFeeDtos;
using System.Data;
namespace StudentManagementSystem.Repositories.StudentMonthlyFeeRepositories
{
    public class StudentMonthlyFeeRepository(IConfiguration _configuration) : IStudentMonthlyFeeRepository
    {
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        
        public async Task<Response<object>> GenerateMonthlyFeesAsync( GenerateMonthlyFeesDto monthlyFeesDto)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGenerateMonthlyFees", connection) { 
                CommandType = CommandType.StoredProcedure,
            };

            command.Parameters.AddWithValue("@year_id", monthlyFeesDto.Year_id);
            command.Parameters.AddWithValue("@fee_month_id", monthlyFeesDto.Fee_month_id);
            command.Parameters.AddWithValue("@due_date", monthlyFeesDto.Due_date);
            
            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            if (result == 0) return new Response<object>(false, "Fees already added for this month");
            return new Response<object>(true, "Successfully added fees for all student");

        }

        public async Task<Response<List<GetUnpaidMonthDto>>> GetUnpaidMonthsByEnrollmentAsync( int enrollment_id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetUnpaidMonthsByEnrollment", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            command.Parameters.AddWithValue("@enrollment_id", enrollment_id);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var unpaidMonthList = new List<GetUnpaidMonthDto>();
            while (await reader.ReadAsync())
            {
                unpaidMonthList.Add(new GetUnpaidMonthDto()
                {
                    Student_fee_id = reader.GetInt32(0),
                    Enrollment_id = reader.GetInt32(1),
                    Year_id = reader.GetInt32(2),
                    Year_label = reader.GetString(3),
                    Month_no = reader.GetInt32(4),
                    Month_name = reader.GetString(5),
                    Fee_amount = reader.GetDecimal(6),
                    Paid_amount = reader.GetDecimal(7),
                    Due_amount = reader.GetDecimal(8),
                    Due_date = DateOnly.FromDateTime(reader.GetDateTime(9)),
                    Payment_status = reader.GetString(10),
                });
            }

            if (unpaidMonthList.Count == 0) return new Response<List<GetUnpaidMonthDto>>(false, "no payment remain");
            return new Response<List<GetUnpaidMonthDto>>(true, "your total due", unpaidMonthList);
        }


        //get feeMonths by year_id
        public async Task<Response<List<FeeMonthDto>>> GetFeeMonthsAsync(int year_id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetFeeMonthsByYear", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@year_id", year_id);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var monthList = new List<FeeMonthDto>();
            while(await reader.ReadAsync())
            {
                monthList.Add(new FeeMonthDto()
                { 
                    Fee_month_id = reader.GetInt32(0),
                    Year_id = reader.GetInt32(1),
                    Month_no = reader.GetInt32(2),
                    Month_name = reader.GetString(3),
                });
            }
            if (monthList.Count == 0) return new Response<List<FeeMonthDto>>(false, "no fee months created");
            return new Response<List<FeeMonthDto>>(true, "all fee month list", monthList);
        }

    }
}
