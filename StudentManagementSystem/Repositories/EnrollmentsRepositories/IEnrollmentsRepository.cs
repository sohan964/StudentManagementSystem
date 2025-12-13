using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.EnrollmentsDtos;

namespace StudentManagementSystem.Repositories.EnrollmentsRepositories
{
    public interface IEnrollmentsRepository
    {
        Task<Response<object>> AddEnrollmentAsync(AddEnrollmentDto addEnrollmentDto);
        Task<Response<List<EnrollmentsDto>>> GetEnrollmentsByParameterAsync(int? year_id, int? class_id, int? section_id, string? status);
    }
}
