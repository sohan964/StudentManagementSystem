using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.TeachersDtos;
using StudentManagementSystem.Repositories.TeachersRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeachersRepository _teachersRepository;

        public TeachersController(ITeachersRepository teachersRepository)
        {
            _teachersRepository = teachersRepository;
        }

        [HttpPost("add-teacher")]
        public async Task<IActionResult> AddTeacher([FromBody] CreateTeacherDto teacherDto)
        {
            var result = await _teachersRepository.AddTeacherAsync(teacherDto);
            return Ok(result);
        }

        [HttpGet("teacher-list")]
        public async Task<IActionResult> GetTeachers()
        {
            var result = await _teachersRepository.GetTeachersAsync();
            return Ok(result);
        }
        [HttpGet("all-info")]
        public async Task<IActionResult> GetAllTeachers()
        {
            var result = await _teachersRepository.GetAllTeachersAsync();
            return Ok(result);
        }

        [HttpGet("GetById/{teacher_id}")]
        public async Task<IActionResult> GetTeacherById([FromRoute]int teacher_id)
        {
            var result = await _teachersRepository.GetTeacherByIdAsync(teacher_id);
            return Ok(result);
        }
        [HttpGet("GetByUserId/{user_id}")]
        public async Task<IActionResult> GetTeacherByUserId( [FromRoute] string user_id)
        {
            var result = await _teachersRepository.GetTeacherByUserIdAsync(user_id);
            return Ok(result);
        }
    }
}
