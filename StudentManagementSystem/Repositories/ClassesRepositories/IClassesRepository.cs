using StudentManagementSystem.Models.ClassesDtos;
using StudentManagementSystem.Models.Components;

namespace StudentManagementSystem.Repositories.ClassesRepositories
{
    public interface IClassesRepository
    {
        Task<Response<List<ClassesDto>>> GetClassesAsync();
        Task<Response<ClassesDto>> GetClassByIdAsync(int class_id);
        Task<Response<object>> AddClassSubjecsAsync(ClassSubjectsDto classSubjectsDto);
        Task<Response<ClassSubjectsDetailsDto>> GetSubjectsByClassIdAsync(int classId);
    }
}
