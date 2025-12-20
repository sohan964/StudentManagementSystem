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

        //get results of a student
        public async Task<Response<OverAllResultDto>> GetFinalResultByEnrollmentAsync(int? enrollment_id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetStudentOverallResults", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@enrollment_id", enrollment_id);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            var overallResult = new OverAllResultDto();
            var subjectResults = new List<EachSubjectResultDto>();

            // ==============================
            // 1️⃣ Read Subject-wise results
            // ==============================
            while (await reader.ReadAsync())
            {
                subjectResults.Add(new EachSubjectResultDto
                {
                    Subject_id = reader.GetInt32(0),
                    Subject_name = reader.GetString(1),
                    Subject_code = reader.GetString(2),
                    Credit_hours = reader.GetInt32(3),
                    Total_marks = reader.GetDecimal(4),
                    Max_marks = reader.GetDecimal(5),
                    Percentage = reader.GetDecimal(6),
                    Grade_name = reader.GetString(7),
                    Grade_point = reader.GetDecimal(8),
                    Weighted_grade_point = reader.GetDecimal(9)
                });
            }

            overallResult.EachSubjectResultDtos = subjectResults;

            // ==============================
            // 2️⃣ Move to Overall result
            // ==============================
            if (await reader.NextResultAsync())
            {
                if (await reader.ReadAsync())
                {
                    overallResult.Enrollment_id = reader.GetInt32(0);
                    overallResult.Student_number = reader.GetString(1);
                    overallResult.Student_name = reader.GetString(2) + " " +reader.GetString(3);
                    overallResult.Class_name = reader.GetString(4);
                    overallResult.Section_name = reader.GetString(5);
                    overallResult.Year_label = reader.GetString(6);
                    overallResult.Total_credit_hours = reader.GetInt32(7);
                    overallResult.Total_weighted_grade_points = reader.GetDecimal(8);
                    overallResult.Gpa = reader.GetDecimal(9);
                    overallResult.Overall_grade = reader.GetString(10);
                }
            }

            return new Response<OverAllResultDto>(true, "Result fetched successfully", overallResult);
        }


    }

}
