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
    }
}
