using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.DaysDtos;

namespace StudentManagementSystem.Repositories.WeeklyDaysRepositories
{
    public interface IWeeklyDaysRepository
    {
        Task<Response<List<DayDto>>> GetWeeklyDaysAsync();
        Task<Response<List<TimeSlotsDto>>> GetTimeSlotsAsync();
    }
}
