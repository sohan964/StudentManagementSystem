using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.ClassRoutineDtos;
using StudentManagementSystem.Repositories.ClassRoutineRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassRoutinesController : ControllerBase
    {
        private readonly IClassRoutineRepository _classRoutineRepository;

        public ClassRoutinesController(IClassRoutineRepository classRoutineRepository)
        {
            _classRoutineRepository = classRoutineRepository;
        }

        [HttpPost("add-routine")]
        public async Task<IActionResult> AddClassRoutine([FromBody] AddClassRoutineDto routineDto)
        {
            var result  = await _classRoutineRepository.AddClassRoutineAsync(routineDto);
            if(!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("teacher-routine/{teacher_id}/{year_id}")]
        public async Task<IActionResult> GetRoutineByTeacher([FromRoute] int teacher_id, [FromRoute] int year_id)
        {
            var result = await _classRoutineRepository.GetTeacherRoutineAsync(teacher_id, year_id);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("class-routine")]
        public async Task<IActionResult> GetRoutineByClassSection([FromQuery] int Year_id, [FromQuery] int Class_id, [FromQuery] int Section_id)
        {
            var result = await _classRoutineRepository.GetRoutineByClassSectionAsync(Year_id, Class_id, Section_id);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
