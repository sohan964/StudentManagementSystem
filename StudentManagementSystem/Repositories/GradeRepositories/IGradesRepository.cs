using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.GradeDtos;

namespace StudentManagementSystem.Repositories.GradeRepositories
{
    public interface IGradesRepository
    {
        Task<Response<List<GradeListsDto>>> GetGradeListAsync();
    }
}
