using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.StudentsDtos;
using System.Data;

namespace StudentManagementSystem.Repositories.StudentsRepositories
{
    public class StudentsRepository(IConfiguration _configuration) : IStudentsRepository
    {
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        
        //add student
        public async Task<Response<object>> AddStudentAsync(CreateStudentDto createStudentDto)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spAddStudent", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@user_id", createStudentDto.User_id);
            command.Parameters.AddWithValue("@first_name", createStudentDto.First_name);
            command.Parameters.AddWithValue("@last_name", createStudentDto.Last_name);
            command.Parameters.AddWithValue("@dob", createStudentDto.DOB);
            command.Parameters.AddWithValue("@gender", createStudentDto.Gender);
            command.Parameters.AddWithValue("@photo", createStudentDto.Photo);
            command.Parameters.AddWithValue("@admission_year", createStudentDto.Admission_year);
            command.Parameters.AddWithValue("@address", createStudentDto.Address);

            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            return new Response<object> (true, $"new student_number add", result);
            
        }

        public async Task<Response<List<StudentInfoDto>>> GetAllStudentsAsync()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetAllStudents", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var studentInfoList = new List<StudentInfoDto>();
            while(await reader.ReadAsync())
            {
                studentInfoList.Add( GetStudentInfo(reader) );  //calling the private method to avoid code duplecation
            }
            return new Response<List<StudentInfoDto>>(true, "all students", studentInfoList);
        }

        public async Task<Response<StudentInfoDto>> GetStudentByIdAsync( int student_id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetStudentById", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@student_id", student_id);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var studentInfo = new StudentInfoDto();
            while( await reader.ReadAsync() )
            {
                studentInfo =  GetStudentInfo(reader); //calling the private method
            }
            return new Response<StudentInfoDto>(true, "the student info", studentInfo);
        }

        public async Task<Response<StudentInfoDto>> GetStudentByUserIdAsync(string user_id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetStudentByUserId", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@user_id", user_id);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var studentInfo = new StudentInfoDto();
            while (await reader.ReadAsync())
            {
                studentInfo = GetStudentInfo(reader); //calling the private method
            }
            return new Response<StudentInfoDto>(true, "the student info", studentInfo);
        }

        private StudentInfoDto GetStudentInfo(SqlDataReader reader)
        {
            return new StudentInfoDto()
            {
                Student_id = reader.GetInt32(0),
                Student_number = reader.GetString(1),
                First_name = reader.GetString(2),
                Last_name = reader.GetString(3),
                DOB = DateOnly.FromDateTime(reader.GetDateTime(4)),
                Gender = reader.GetString(5),
                Photo = reader.GetString(6),
                Admission_year = reader.GetInt32(7),
                Address = reader.GetString(8),
                Class_id = reader.IsDBNull(9) ? null : reader.GetInt32(9),
                Class_name = reader.IsDBNull(10) ? null : reader.GetString(10),
                Section_id = reader.IsDBNull(11) ? null : reader.GetInt32(11),
                Section_name = reader.IsDBNull(12) ? null : reader.GetString(12),

                //users table
                User_id = reader.GetString(13),
                UserName = reader.GetString(14),
                Email = reader.GetString(15),
                PhoneNumber = reader.IsDBNull(16) ? null : reader.GetString(16),
                Current_enrollment_id = reader.IsDBNull(17) ? null : reader.GetInt32(17),
                Current_year_id = reader.IsDBNull(18) ? null : reader.GetInt32(18),
            };
        }

       
    }
}
