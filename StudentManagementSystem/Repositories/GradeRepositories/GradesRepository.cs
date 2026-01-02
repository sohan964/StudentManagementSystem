using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.GradeDtos;
using System.Data;
namespace StudentManagementSystem.Repositories.GradeRepositories
{
    public class GradesRepository(IConfiguration _configuration) : IGradesRepository
    {
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

        public async Task<Response<List<GradeListsDto>>> GetGradeListAsync()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetGradeList", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var gradeList = new List<GradeListsDto>();
            while (await reader.ReadAsync())
            {
                gradeList.Add(new GradeListsDto()
                {
                    Grade_id = reader.GetInt32(0),
                    Grade_name = reader.GetString(1),
                    Min_mark = reader.GetInt32(2),
                    Max_mark = reader.GetInt32(3),
                    Grade_point = reader.GetDecimal(4)
                });
            }
            if (gradeList.Count == 0) return new Response<List<GradeListsDto>>(false, "Not found");
            return new Response<List<GradeListsDto>>(true, "All grades", gradeList);
        }
    }
}
