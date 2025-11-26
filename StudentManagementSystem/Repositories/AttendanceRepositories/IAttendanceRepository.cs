using StudentManagementSystem.Models.AttendanceDtos;
using StudentManagementSystem.Models.Components;

namespace StudentManagementSystem.Repositories.AttendanceRepositories
{
    public interface IAttendanceRepository
    {
        Task<Response<object>> TakeAttendaceAsync(TakeAttendancesDto takeAttendances);
    }
}
