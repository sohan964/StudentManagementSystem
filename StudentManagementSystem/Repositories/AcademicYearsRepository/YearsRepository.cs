using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models.AcademicYearsDtos;
using StudentManagementSystem.Models.Components;
using System.Data;
namespace StudentManagementSystem.Repositories.AcademicYearsRepository
{
    public class YearsRepository(IConfiguration _configuration) : IYearsRepository
    {
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

        public async Task<Response<object>> AddAcademicYearAsync(AddAcademicYearDto academicYear)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spAddAcademicYear", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@year_label", academicYear.Year_lable);
            command.Parameters.AddWithValue("@start_date", academicYear.Start_date);
            command.Parameters.AddWithValue("@end_date", academicYear.End_date);
            command.Parameters.AddWithValue("@is_active", academicYear.Is_active);

            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            return new Response<object>(true, $"year add {result}", result);
        }

        public async Task<Response<List<AcademicYearDto>>> GetAcademicYearsAsync()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetAcademicYears", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var yearList = new List<AcademicYearDto>();
            while(await reader.ReadAsync())
            {
                yearList.Add(new AcademicYearDto()
                {
                    Year_id = reader.GetInt32(0),
                    Year_lable = reader.GetString(1),
                    Start_date = DateOnly.FromDateTime(reader.GetDateTime(2)),
                    End_date = DateOnly.FromDateTime(reader.GetDateTime(3)),
                    Is_active = reader.GetBoolean(4),
                });
            }
            return new Response<List<AcademicYearDto>>(true, "academic year list", yearList);
        }
    }
}
