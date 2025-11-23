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
            command.Parameters.AddWithValue("@department_id", createSectionDto.Department_id);
            await connection.OpenAsync();
            // Execute stored procedure and get return message
            var result = await command.ExecuteScalarAsync();
            string message = result?.ToString() ?? "No message returned.";

            return new Response<object>(true, message, result);
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
                    Capacity = reader.GetInt32(3),
                    Department_id = reader.GetInt32(4)
                });
            }
            return new Response<List<SectionsDto>>(true, "all sections", sectionList);
        }
        public async Task<Response<List<SectionsDto>>> GetSectionsByDepartmentIdAsync(int departmentId)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetSectionsByDepartmentId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@department_id", departmentId);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var sectionList = new List<SectionsDto>();
            while (await reader.ReadAsync())
            {
                sectionList.Add(new SectionsDto()
                {
                    Section_id = reader.GetInt32(0),
                    Class_id = reader.GetInt32(1),
                    Section_name = reader.GetString(2),
                    Capacity = reader.GetInt32(3),
                    Department_id = reader.GetInt32(4)
                });
            }
            return new Response<List<SectionsDto>>(true, "Section under selected department", sectionList);

        }
    }
}
