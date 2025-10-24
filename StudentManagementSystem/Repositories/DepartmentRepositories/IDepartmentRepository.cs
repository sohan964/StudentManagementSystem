using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.DepartmentDtos;

namespace StudentManagementSystem.Repositories.DepartmentRepositories
{
    public interface IDepartmentRepository
    {
        Task<Response<List<DepartmentDto>>> GetDepartmentsAsync();
        Task<Response<DepartmentDto>> GetDepartmentByIdAsync(int id);
        Task<Response<object>> AddDepartmentAsync(CreateDepartmentDto createDepartmentDto);
        Task<Response<object>> UpdateDepartmentAsync(DepartmentDto departmentDto);
    }
}
