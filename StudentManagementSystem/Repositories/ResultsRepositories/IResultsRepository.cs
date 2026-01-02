using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.ResultsDtos;

namespace StudentManagementSystem.Repositories.ResultsRepositories
{
    public interface IResultsRepository
    {
        Task<Response<object>> AddResultsAsync(AddResultDto addResult);
        Task<Response<StudentSubjectResultDto>> GetResultBySessionAndEnrollmentAsync(int? exam_session_id, int? enrollment_id);
        Task<Response<OverAllResultDto>> GetFinalResultByEnrollmentAsync(int? enrollment_id);
        Task<Response<List<SubjectTotalResult>>> GetAllSubjectsTotalResultAsync(int enrollment_id);
        Task<Response<List<SubjectResultsDto>>> GetSubjectDetailResultsAsync(int enrollment_id, int subject_id);

    }
}
