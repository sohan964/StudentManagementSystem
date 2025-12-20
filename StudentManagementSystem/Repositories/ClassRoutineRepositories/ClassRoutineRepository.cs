using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using StudentManagementSystem.Models.ClassRoutineDtos;
using StudentManagementSystem.Models.Components;
using System.Data;
namespace StudentManagementSystem.Repositories.ClassRoutineRepositories
{
    public class ClassRoutineRepository(IConfiguration _configuration) : IClassRoutineRepository
    {
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

        //addroutine
        public async Task<Response<object>> AddClassRoutineAsync(AddClassRoutineDto addRoutineDto)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spAddClassRoutine", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@year_id", addRoutineDto.Year_id);
            command.Parameters.AddWithValue("@class_id", addRoutineDto.Class_id);
            command.Parameters.AddWithValue("@section_id", addRoutineDto.Section_id);
            command.Parameters.AddWithValue("@subject_id", addRoutineDto.Subject_id);
            command.Parameters.AddWithValue("@teacher_id", addRoutineDto.Teacher_id);
            command.Parameters.AddWithValue("@day_id", addRoutineDto.Day_id);
            command.Parameters.AddWithValue("@slot_id", addRoutineDto.Slot_id);

            try
            {
                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                return new Response<object>(true, "class routine added successfully", result);
            }catch(SqlException ex)
            {
                return new Response<object>(false, ex.Message);
            }
        }

        public async Task<Response<List<TeacherRoutineDto>>> GetTeacherRoutineAsync(int Teacher_id, int Year_id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetRoutineByTeacher", connection)
            { 
              CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@teacher_id", Teacher_id);
            command.Parameters.AddWithValue("@year_id", Year_id);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var teacherRoutine = new List<TeacherRoutineDto>();
            while(await  reader.ReadAsync())
            {
                teacherRoutine.Add(new TeacherRoutineDto
                {
                    Routine_id = reader.GetInt32(0),
                    Day_name = reader.GetString(1),
                    Slot_number = reader.GetInt32(2),
                    Start_time = reader.GetString(3),
                    End_time = reader.GetString(4),
                    Subject_name = reader.GetString(5),
                    Class_name = reader.GetString(6),
                    Section_name = reader.GetString(7),
                    Year_id = reader.GetInt32(8),
                    Class_id = reader.GetInt32(9),
                    Section_id = reader.GetInt32(10),
                    Subject_id = reader.GetInt32(11)
                });
            }
            if (teacherRoutine.IsNullOrEmpty())
            {
                return new Response<List<TeacherRoutineDto>>(false, "No class Routine found");
            }
            return new Response<List<TeacherRoutineDto>>(true, "Teacher Routine found", teacherRoutine);
        }

        public async Task<Response<List<TeacherRoutineDto>>> GetRoutineByClassSectionAsync(int Year_id, int Class_id, int Section_id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetRoutineByClassSection", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            
            command.Parameters.AddWithValue("@year_id", Year_id);
            command.Parameters.AddWithValue("@class_id", Class_id);
            command.Parameters.AddWithValue("@section_id", Section_id);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var teacherRoutine = new List<TeacherRoutineDto>();
            while (await reader.ReadAsync())
            {
                teacherRoutine.Add(new TeacherRoutineDto
                {
                    Routine_id = reader.GetInt32(0),
                    Day_name = reader.GetString(1),
                    Slot_number = reader.GetInt32(2),
                    Start_time = reader.GetString(3),
                    End_time = reader.GetString(4),
                    Subject_id = reader.GetInt32(5),
                    Subject_name = reader.GetString(6),
                    Class_id = reader.GetInt32(7),
                    Class_name = reader.GetString(8),
                    Section_id = reader.GetInt32(9),
                    Section_name = reader.GetString(10),

                    Year_id = reader.GetInt32(12)
                    
                });
            }
            if (teacherRoutine.IsNullOrEmpty())
            {
                return new Response<List<TeacherRoutineDto>>(false, "No class Routine found");
            }
            return new Response<List<TeacherRoutineDto>>(true, "Your Routine", teacherRoutine);
        }


    }
}
