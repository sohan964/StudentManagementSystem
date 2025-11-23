using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.EnrollmentsDtos;

namespace StudentManagementSystem.Repositories.EnrollmentsRepositories
{
    public interface IEnrollmentsRepository
    {
        Task<Response<object>> AddEnrollmentAsync(AddEnrollmentDto addEnrollmentDto);
    }
}
