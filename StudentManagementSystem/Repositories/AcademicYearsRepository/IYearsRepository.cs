using StudentManagementSystem.Models.AcademicYearsDtos;
using StudentManagementSystem.Models.Components;

namespace StudentManagementSystem.Repositories.AcademicYearsRepository
{
    public interface IYearsRepository
    {
        Task<Response<object>> AddAcademicYearAsync(AddAcademicYearDto academicYear);
        Task<Response<List<AcademicYearDto>>> GetAcademicYearsAsync();
    }
}
