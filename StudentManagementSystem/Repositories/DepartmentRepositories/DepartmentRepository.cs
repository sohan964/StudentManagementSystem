using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.DepartmentDtos;
using System.Data;

namespace StudentManagementSystem.Repositories.DepartmentRepositories
{
    public class DepartmentRepository(IConfiguration _configuration) : IDepartmentRepository
    {
        
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        public async Task<Response<List<DepartmentDto>>> GetDepartmentsAsync()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetDepartments", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            List<DepartmentDto> departments = new List<DepartmentDto>();
            while (await reader.ReadAsync())
            {
                departments.Add(new DepartmentDto()
                {
                    Department_id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Code = reader.GetString(2),
                    Description = reader.GetString(3)
                });
            }
            return new Response<List<DepartmentDto>> (true, "All the department List", departments);
        }

        //get by id
        public async Task<Response<DepartmentDto>> GetDepartmentByIdAsync(int id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetDepartmentById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Department_id", id);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var department = new DepartmentDto();
            while(await reader.ReadAsync())
            {
                department = new DepartmentDto()
                {
                    Department_id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Code = reader.GetString(2),
                    Description = reader.GetString(3)
                };
            }
            return new Response<DepartmentDto> (true, $"about {department?.Name}", department);

        }

        public async Task<Response<object>> AddDepartmentAsync(CreateDepartmentDto createDepartmentDto)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spAddDepartment", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Name", createDepartmentDto.Name);
            command.Parameters.AddWithValue("@Code", createDepartmentDto?.Code);
            command.Parameters.AddWithValue("@Description", createDepartmentDto?.Description);
            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            return new Response<object>(true, "added Success",result);
        }

        public async Task<Response<object>> UpdateDepartmentAsync(DepartmentDto departmentDto)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spUpdateDepartment", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Department_id", departmentDto.Department_id);
            command.Parameters.AddWithValue("@Name", departmentDto?.Name);
            command.Parameters.AddWithValue("@Code", departmentDto?.Code);
            command.Parameters.AddWithValue("@description", departmentDto?.Description);
            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            return new Response<object>(true, "updated Success", result);
        }


    }
}
