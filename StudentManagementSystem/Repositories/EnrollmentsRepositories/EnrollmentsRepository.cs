using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.EnrollmentsDtos;
using System.Data;

namespace StudentManagementSystem.Repositories.EnrollmentsRepositories
{
    public class EnrollmentsRepository(IConfiguration _configuration) : IEnrollmentsRepository
    {
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        
        //add new enrollment
        public async Task<Response<object>> AddEnrollmentAsync(AddEnrollmentDto addEnrollmentDto)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spAddEnrollment", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@student_id",addEnrollmentDto.Student_id);
            command.Parameters.AddWithValue("@year_id", addEnrollmentDto.Year_id);
            command.Parameters.AddWithValue("@class_id", addEnrollmentDto.Class_id);
            command.Parameters.AddWithValue("@section_id", addEnrollmentDto.Section_id);
            command.Parameters.AddWithValue("@admission_date", addEnrollmentDto.Admission_date);
            command.Parameters.AddWithValue("@status", addEnrollmentDto.Status);


            try
            {
                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync(); // because SP returns SCOPE_IDENTITY()
                return new Response<object>(true, "Enrollment added successfully", result);
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("already enrolled"))
                {
                    return new Response<object>(false, "Student is already enrolled for this academic year.");
                }

                // Other SQL errors
                return new Response<object>(false, $"Database error: {ex.Message}");
            }

        }

        //get enrollments by year_id, class_id, section_id, status
        public async Task<Response<List<EnrollmentsDto>>> GetEnrollmentsByParameterAsync(int? year_id, int? class_id, int? section_id, string? status)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("spGetEnrollments", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@year_id", year_id);
            command.Parameters.AddWithValue("@class_id", class_id);
            command.Parameters.AddWithValue("@section_id", section_id);
            command.Parameters.AddWithValue("@status", status);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var enrollList = new List<EnrollmentsDto>();
            while(await reader.ReadAsync())
            {
                enrollList.Add(new EnrollmentsDto() 
                { 
                    Enrollment_id = reader.GetInt32(0),
                    Student_id = reader.GetInt32(1),
                    Student_name = reader.GetString(2) + " " + reader.GetString(3),
                    Student_number = reader.GetString(4),
                    Year_id = reader.GetInt32(5),
                    Class_id = reader.GetInt32(6),
                    Section_id = reader.GetInt32(7),
                    Status = reader.GetString(8)
                });
            }
            return new Response<List<EnrollmentsDto>>(true, "enrollments list of this section", enrollList);
        }
    }
}
