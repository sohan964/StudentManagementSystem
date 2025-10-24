using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.SubjectDtos;

namespace StudentManagementSystem.Repositories.SubjectsRepositories
{
    public interface ISubjectsRepository
    {
        Task<Response<List<SubjectsDto>>> GetSubjectsAsync();
        Task<Response<SubjectsDto>> GetSubjectByIdAsync(int id);
        Task<Response<object>> AddSubjectAsync(CreateSubjectDto subject);
    }
}
