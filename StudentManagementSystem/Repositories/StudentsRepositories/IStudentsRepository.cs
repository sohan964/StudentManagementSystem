using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.StudentsDtos;

namespace StudentManagementSystem.Repositories.StudentsRepositories
{
    public interface IStudentsRepository
    {
        Task<Response<object>> AddStudentAsync(CreateStudentDto createStudentDto);
        Task<Response<List<StudentInfoDto>>> GetAllStudentsAsync();
        Task<Response<StudentInfoDto>> GetStudentByIdAsync(int student_id);
    }
}
