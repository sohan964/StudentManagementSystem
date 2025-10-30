using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.TeachersDtos;
using System.Data;

namespace StudentManagementSystem.Repositories.TeachersRepositories
{
    public class TeachersRepository(IConfiguration _configuration) : ITeachersRepository
    {
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

        //add new teacher
        public async Task<Response<object>> AddTeacherAsync(CreateTeacherDto createTeacherDto)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spAddTeacher", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@user_id", createTeacherDto.User_id);
            command.Parameters.AddWithValue("@teacher_code", createTeacherDto.Teacher_code);
            command.Parameters.AddWithValue("@first_name", createTeacherDto.First_name);
            command.Parameters.AddWithValue("@last_name", createTeacherDto.Last_name);
            command.Parameters.AddWithValue("@department_id", createTeacherDto.Department_id);
            command.Parameters.AddWithValue("@contact", createTeacherDto.Contact);
            command.Parameters.AddWithValue("@hire_date", DateOnly.FromDateTime(DateTime.Today));
            command.Parameters.AddWithValue("@photo", createTeacherDto.Photo);

            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            return new Response<object>(true, "teacher successfully added", result);
        }

        public async Task<Response<List<TeachersDto>>> GetTeachersAsync()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetTeachers", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var teacherList  = new List<TeachersDto>();
            while(await reader.ReadAsync())
            {
                teacherList.Add(new TeachersDto()
                {
                    Teacher_id = reader.GetInt32(0),
                    User_id = reader.GetString(1),
                    Teacher_code = reader.GetString(2),
                    First_name = reader.GetString(3),
                    Last_name = reader.GetString(4),
                    Department_id = reader.GetInt32(5),
                    Contact = reader.GetString(6),
                    Hire_date = DateOnly.FromDateTime(reader.GetDateTime(7)),
                    Photo = reader.GetString(8),
                });
            }
            return new Response<List<TeachersDto>>(true, "all teachers list", teacherList);
        }

        public async Task<Response<List<TeacherInfoDto>>> GetAllTeachersAsync()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetAllTeachers", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var teacherList = new List<TeacherInfoDto>();
            while(await reader.ReadAsync())
            {
                teacherList.Add(new TeacherInfoDto()
                {
                    Teacher_id = reader.GetInt32(0),
                    Teacher_code = reader.GetString(1),
                    First_name = reader.GetString(2),
                    Last_name = reader.GetString(3),
                    Department_id=reader.GetInt32(4),
                    Department_name = reader.GetString(5),
                    Contact = reader.GetString(6),
                    Hire_date = DateOnly.FromDateTime(reader.GetDateTime(7)),
                    Photo = reader.GetString(8),
                    User_id = reader.GetString(9),
                    UserName = reader.GetString(10),
                    Email = reader.GetString(11),
                    PhoneNumber = reader.IsDBNull(12) ? null : reader.GetString(12)
                });
            }
            return new Response<List<TeacherInfoDto>>(true, "All Teachers Informations", teacherList);
        }

        //get by teachers id
        public async Task<Response<TeacherInfoDto>> GetTeacherByIdAsync(int teacher_id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetTeacherById", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@teacher_id", teacher_id);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var teacher = new TeacherInfoDto();
            while(await reader.ReadAsync())
            {
                teacher = new TeacherInfoDto() 
                {
                    Teacher_id = reader.GetInt32(0),
                    Teacher_code = reader.GetString(1),
                    First_name = reader.GetString(2),
                    Last_name = reader.GetString(3),
                    Department_id = reader.GetInt32(4),
                    Department_name = reader.GetString(5),
                    Contact = reader.GetString(6),
                    Hire_date = DateOnly.FromDateTime(reader.GetDateTime(7)),
                    Photo = reader.GetString(8),
                    User_id = reader.GetString(9),
                    UserName = reader.GetString(10),
                    Email = reader.GetString(11),
                    PhoneNumber = reader.IsDBNull(12) ? null : reader.GetString(12)
                };
            }
            return new Response<TeacherInfoDto>(true, "the teacher", teacher);
        }

    }
}
