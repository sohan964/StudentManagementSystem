using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.ResultsDtos;
using System.Data;
namespace StudentManagementSystem.Repositories.ResultsRepositories
{
    public class ResultsRepository(IConfiguration _configuration) : IResultsRepository
    {
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

        //add Result
        public async Task<Response<object>> AddResultsAsync(AddResultDto addResult)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spAddExamResult", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@exam_session_id", addResult.Exam_session_id);
            command.Parameters.AddWithValue("@enrollment_id", addResult.Enrollment_id);
            command.Parameters.AddWithValue("@obtained_marks", addResult.Obtained_marks);

            await connection.OpenAsync();
            var res = await command.ExecuteScalarAsync();
            return new Response<object>(true, "Added success", res);
        }

        //get result by session_id and enrollment_id
        public async Task<Response<StudentSubjectResultDto>> GetResultBySessionAndEnrollmentAsync(int? exam_session_id, int? enrollment_id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetExamResult", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@exam_session_id", exam_session_id);
            command.Parameters.AddWithValue("@enrollment_id", enrollment_id);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var subjectResult = new StudentSubjectResultDto();
            while(await reader.ReadAsync())
            {
                subjectResult = new StudentSubjectResultDto()
                {
                    Result_id = reader.GetInt32(0),
                    Exam_session_id = reader.GetInt32(1),
                    Enrollment_id = reader.GetInt32(2),
                    obtained_marks = reader.GetDecimal(3)
                };
            }
            if(subjectResult.Result_id == null)
            {
                return new Response<StudentSubjectResultDto>(false, "no result ");
            }
            return new Response<StudentSubjectResultDto>(true, "result found", subjectResult);
        }
    }
    
}
