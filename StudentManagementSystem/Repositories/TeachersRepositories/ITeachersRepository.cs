using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.TeachersDtos;

namespace StudentManagementSystem.Repositories.TeachersRepositories
{
    public interface ITeachersRepository
    {
        Task<Response<object>> AddTeacherAsync(CreateTeacherDto createTeacherDto);
        Task<Response<List<TeachersDto>>> GetTeachersAsync();
        Task<Response<List<TeacherInfoDto>>> GetAllTeachersAsync();
        Task<Response<TeacherInfoDto>> GetTeacherByIdAsync(int teacher_id);
    }
}
