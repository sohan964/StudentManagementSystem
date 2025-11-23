using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.SectionsDtos;

namespace StudentManagementSystem.Repositories.SectionsRepositories
{
    public interface ISectionsRepository
    {
        Task<Response<object>> AddSectionAsync(CreateSectionDto createSectionDto);
        Task<Response<List<SectionsDto>>> GetSectionsAsync();
        Task<Response<List<SectionsDto>>> GetSectionsByDepartmentIdAsync(int departmentId);
    }
}
