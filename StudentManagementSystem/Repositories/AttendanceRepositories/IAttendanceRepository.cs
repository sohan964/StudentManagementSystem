using StudentManagementSystem.Models.AttendanceDtos;
using StudentManagementSystem.Models.Components;

namespace StudentManagementSystem.Repositories.AttendanceRepositories
{
    public interface IAttendanceRepository
    {
        Task<Response<object>> TakeAttendaceAsync(TakeAttendancesDto takeAttendances);
        Task<Response<List<GetAttendancesDto>>> GetAttendanceSummaryAsync(int? year_id, int? class_id, int? section_id, int? subject_id);
        Task<Response<GetAttendanceDetailsDto>> GetAttendanceDetailsAsync(int enrollment_id, int year_id, int subject_id);
        Task<Response<object>> UpdateAttendanceByRecordAsync(int record_id, string status);
    }
}
