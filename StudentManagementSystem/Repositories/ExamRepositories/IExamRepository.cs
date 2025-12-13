using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.ExamDtos;

namespace StudentManagementSystem.Repositories.ExamRepositories
{
    public interface IExamRepository
    {
        Task<Response<object>> AddExamSessionAsync(CreateExamSessionDto createExamSessionDto);
        Task<Response<List<GetExamTypesDto>>> GetExamTypesAsync();
        Task<Response<List<GetExamSlotsDto>>> GetExamSlotsAsync();
        Task<Response<List<GetExamSessionDto>>> GetExamSessionsAsync(int? exam_session_id,
            int? year_id, int? exam_type_id, int? subject_id, int? class_id, int? section_id);

    }
}
