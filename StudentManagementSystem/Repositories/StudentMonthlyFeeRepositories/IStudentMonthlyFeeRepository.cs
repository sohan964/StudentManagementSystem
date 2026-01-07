using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.StudentMonthlyFeeDtos;

namespace StudentManagementSystem.Repositories.StudentMonthlyFeeRepositories
{
    public interface IStudentMonthlyFeeRepository
    {
        Task<Response<object>> GenerateMonthlyFeesAsync(GenerateMonthlyFeesDto monthlyFeesDto);
        Task<Response<List<GetUnpaidMonthDto>>> GetUnpaidMonthsByEnrollmentAsync(int enrollment_id);
        Task<Response<List<FeeMonthDto>>> GetFeeMonthsAsync(int year_id);
    }
}
