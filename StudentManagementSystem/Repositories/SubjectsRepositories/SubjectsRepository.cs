using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.SubjectDtos;
using System.Data;


namespace StudentManagementSystem.Repositories.SubjectsRepositories
{
    public class SubjectsRepository(IConfiguration _configuration) : ISubjectsRepository
    {
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

        public async Task<Response<List<SubjectsDto>>> GetSubjectsAsync()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetSubjects", connection) { 
                CommandType = CommandType.StoredProcedure
            };
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var subjectlist = new List<SubjectsDto>();
            while(await  reader.ReadAsync())
            {
                subjectlist.Add(new SubjectsDto()
                {
                    Subject_id = reader.GetInt32(0),
                    Subject_code = reader.GetString(1),
                    Name = reader.GetString(2),
                    Is_theory = reader.GetBoolean(3),
                    Is_practical = reader.GetBoolean(4),
                    Default_marks = reader.GetInt32(5),
                    Department_id = reader.GetInt32(6),
                    Credit_hours = reader.GetInt32(7),
                });
            }
            return new Response<List<SubjectsDto>>(true, "All Subjects List", subjectlist);

        }

        public async Task<Response<SubjectsDto>> GetSubjectByIdAsync(int id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetSubjectById", connection)
            {
                CommandType = CommandType.StoredProcedure 
            };
            command.Parameters.AddWithValue("@subject_id", id);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var subject = new SubjectsDto();
            while(await reader.ReadAsync())
            {
                subject = new SubjectsDto()
                {
                    Subject_id = reader.GetInt32(0),
                    Subject_code = reader.GetString(1),
                    Name = reader.GetString(2),
                    Is_theory = reader.GetBoolean(3),
                    Is_practical = reader.GetBoolean(4),
                    Default_marks = reader.GetInt32(5),
                    Department_id = reader.GetInt32(6),
                    Credit_hours = reader.GetInt32(7),
                };
            }
            return new Response<SubjectsDto>(true, "your subject", subject);

        }

        public async Task<Response<object>> AddSubjectAsync(CreateSubjectDto subject)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spAddSubject", connection)
            {
                CommandType= CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Subject_code", subject.Subject_code);
            command.Parameters.AddWithValue("@Name", subject.Name);
            command.Parameters.AddWithValue("@Is_theory", subject.Is_theory);
            command.Parameters.AddWithValue("@Is_practical",subject.Is_practical);
            command.Parameters.AddWithValue("@Default_marks", subject.Default_marks);
            command.Parameters.AddWithValue("@department_id", subject.Department_id);
            command.Parameters.AddWithValue("@credit_hours", subject.Credit_hours);
            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            return new Response<object>(true, "subject add successfully", result);
        }
    }
}
