using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.ExamDtos;
using System.Data;
namespace StudentManagementSystem.Repositories.ExamRepositories
{
    public class ExamRepository(IConfiguration _configuration) : IExamRepository
    {
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

        //add examsession
        public async Task<Response<object>> AddExamSessionAsync(CreateExamSessionDto createExamSessionDto)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spAddExamSession", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@year_id", createExamSessionDto.Year_id);
            command.Parameters.AddWithValue("@exam_type_id", createExamSessionDto.Exam_type_id);
            command.Parameters.AddWithValue("@subject_id", createExamSessionDto.Subject_id);
            command.Parameters.AddWithValue("@class_id", createExamSessionDto.Class_id);
            command.Parameters.AddWithValue("@section_id", createExamSessionDto.Section_id);
            command.Parameters.AddWithValue("@exam_date", createExamSessionDto.Exam_date);
            command.Parameters.AddWithValue("@exam_slot_id", createExamSessionDto.Exam_slot_id);
            command.Parameters.AddWithValue("@max_marks", createExamSessionDto.Max_marks);
            try
            {
                await connection.OpenAsync();
                var res = await command.ExecuteScalarAsync();
                if (res == null || res.Equals(0))  new Response<object>(false, "fail to add", res);
                return new Response<object>(true, "Add successfully", res);
            }catch (SqlException ex) 
            {
                if (ex.Message.Contains("already"))
                {
                    return new Response<object>(false, ex.Message);
                }
                return new Response<object>(false, ex.Message);
            }
            
        }
        //get exam sesstions
        public async Task<Response<List<GetExamSessionDto>>> GetExamSessionsAsync(int? exam_session_id, 
            int? year_id, int? exam_type_id, int? subject_id, int? class_id, int? section_id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetExamSessions", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@exam_session_id", exam_session_id);
            command.Parameters.AddWithValue("@year_id", year_id);
            command.Parameters.AddWithValue("@exam_type_id", exam_type_id);
            command.Parameters.AddWithValue("@subject_id", subject_id);
            command.Parameters.AddWithValue("@class_id", class_id);
            command.Parameters.AddWithValue("@section_id", section_id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var sessionList = new List<GetExamSessionDto>();
            while(await reader.ReadAsync())
            {
                sessionList.Add(new GetExamSessionDto()
                {
                    Exam_session_id = reader.GetInt32(0),
                    Year_id = reader.GetInt32(1),
                    Exam_type_id = reader.GetInt32(2),
                    Exam_type_name = reader.GetString(3),
                    Subject_id = reader.GetInt32(4),
                    Subject_name = reader.GetString(5),
                    Class_id = reader.GetInt32(6),
                    Section_id = reader.GetInt32(7),
                    Exam_date = DateOnly.FromDateTime(reader.GetDateTime(8)),
                    Max_marks = reader.GetDecimal(9),
                    Exam_slot_id = reader.GetInt32(10)
                });
            }
            return new Response<List<GetExamSessionDto>>(true, "Exam Sessions", sessionList);
        }

        public async Task<Response<List<GetExamTypesDto>>> GetExamTypesAsync()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetExamTypes", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var typeList = new List<GetExamTypesDto>();
            while(await reader.ReadAsync())
            {
                typeList.Add(new GetExamTypesDto()
                {
                    Exam_type_id = reader.GetInt32(0),
                    Type_name = reader.GetString(1),
                    Weight_percentage = reader.GetDecimal(2),
                });
            }
            return new Response<List<GetExamTypesDto>>(true, "All Exam Types", typeList);
        }

        //get Exam_slots
        public async Task<Response<List<GetExamSlotsDto>>> GetExamSlotsAsync()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetExamSlots", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var examSlotsList = new List<GetExamSlotsDto>();
            while(await reader.ReadAsync())
            {
                examSlotsList.Add(new GetExamSlotsDto()
                {
                    Exam_slot_id = reader.GetInt32(0),
                    Exam_slot_name = reader.GetString(1),
                    Exam_start_time = reader.GetString(2),
                    Exam_end_time = reader.GetString(3),
                });
            }

            return new Response<List<GetExamSlotsDto>>(true, "All Exam Slots list", examSlotsList);
        }
    }
}
