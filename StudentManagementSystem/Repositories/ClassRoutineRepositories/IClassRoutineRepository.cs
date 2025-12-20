using StudentManagementSystem.Models.ClassRoutineDtos;
using StudentManagementSystem.Models.Components;

namespace StudentManagementSystem.Repositories.ClassRoutineRepositories
{
    public interface IClassRoutineRepository
    {
        Task<Response<object>> AddClassRoutineAsync(AddClassRoutineDto addRoutineDto);
        Task<Response<List<TeacherRoutineDto>>> GetTeacherRoutineAsync(int Teacher_id, int Year_id);
        Task<Response<List<TeacherRoutineDto>>> GetRoutineByClassSectionAsync(int Year_id, int Class_id, int Section_id);
    }
}
