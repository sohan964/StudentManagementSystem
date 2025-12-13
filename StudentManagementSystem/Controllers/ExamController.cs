using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.ExamDtos;
using StudentManagementSystem.Repositories.ExamRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamRepository _examRepository;

        public ExamController(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        [HttpPost("add-exam")]
        public async Task<IActionResult> AddExamSession([FromBody] CreateExamSessionDto dto)
        {
            var result = await _examRepository.AddExamSessionAsync(dto);
            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("get-exam-types")]
        public async Task<IActionResult> GetExamTypes()
        {
            var result = await _examRepository.GetExamTypesAsync();
            return Ok(result);
        }

        [HttpGet("get-exam-slots")]
        public async Task<IActionResult> GetExamSlots()
        {
            var result = await _examRepository.GetExamSlotsAsync();
            return Ok(result);
        }

        [HttpGet("get-exam-sessions")]
        public async Task<IActionResult> GetExamSessions([FromQuery]int? exam_session_id,
            [FromQuery] int? year_id, [FromQuery] int? exam_type_id, [FromQuery] int? subject_id,
            [FromQuery] int? class_id, [FromQuery] int? section_id)
        {
            var result = await _examRepository.GetExamSessionsAsync(exam_session_id, year_id, exam_type_id, subject_id, class_id, section_id);
            return Ok(result);
        }
        


    }
}
