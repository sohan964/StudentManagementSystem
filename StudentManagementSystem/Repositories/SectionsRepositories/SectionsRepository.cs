using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.SectionsDtos;
using System.Data;

namespace StudentManagementSystem.Repositories.SectionsRepositories
{
    public class SectionsRepository(IConfiguration _configuration) : ISectionsRepository
    {
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

        //add
        public async Task<Response<object>> AddSectionAsync(CreateSectionDto createSectionDto)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spAddSection", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@class_id", createSectionDto.Class_id);
            command.Parameters.AddWithValue("@section_name", createSectionDto.Section_name);
            command.Parameters.AddWithValue("@capacity", createSectionDto.Capacity);
            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            return new Response<object>(true, $"Section added succes{result}", result);
        }

        //get all
        public async Task<Response<List<SectionsDto>>> GetSectionsAsync()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetSections", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var sectionList = new List<SectionsDto>();
            while(await reader.ReadAsync())
            {
                sectionList.Add(new SectionsDto()
                {
                    Section_id = reader.GetInt32(0),
                    Class_id = reader.GetInt32(1),
                    Section_name = reader.GetString(2),
                    Capacity = reader.GetInt32(3)
                });
            }
            return new Response<List<SectionsDto>>(true, "all sections", sectionList);
        }
    }
}
