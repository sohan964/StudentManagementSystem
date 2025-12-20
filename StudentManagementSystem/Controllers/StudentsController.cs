using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.StudentsDtos;
using StudentManagementSystem.Repositories.StudentsRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentsRepository _studentsRepository;

        public StudentsController(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        //add
        [HttpPost("add-student")]
        public async Task<IActionResult> AddStudent([FromBody] CreateStudentDto createStudent)
        {
            var result = await _studentsRepository.AddStudentAsync(createStudent);
            return Ok(result);
        }

        //get all student
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllStudents()
        {
            var result = await _studentsRepository.GetAllStudentsAsync();
            return Ok(result);
        }

        //get studentbyid
        [HttpGet("GetById/{student_id}")]
        public async Task<IActionResult> GetStudentById([FromRoute] int student_id)
        {
            var result = await _studentsRepository.GetStudentByIdAsync(student_id);
            return Ok(result);
        }

        [HttpGet("GetByUserId/{user_id}")]
        public async Task<IActionResult> GetStudentByUserId([FromRoute] string user_id)
        {
            var result = await _studentsRepository.GetStudentByUserIdAsync(user_id);
            return Ok(result);
        }


    }
}
