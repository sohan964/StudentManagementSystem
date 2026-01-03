using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.StudentMonthlyFeeDtos;
using StudentManagementSystem.Repositories.StudentMonthlyFeeRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentFeesController : ControllerBase
    {
        private readonly IStudentMonthlyFeeRepository _studentMonthlyFeeRepository;

        public StudentFeesController(IStudentMonthlyFeeRepository studentMonthlyFeeRepository)
        {
            _studentMonthlyFeeRepository = studentMonthlyFeeRepository;
        }

        [HttpPost("add-students-fees")]
        public async Task<IActionResult> GenerateMonthlyFees([FromBody] GenerateMonthlyFeesDto monthlyFeesDto)
        {
            var result = await _studentMonthlyFeeRepository.GenerateMonthlyFeesAsync(monthlyFeesDto);
            if(!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("get-student-due/{enrollment_id}")]
        public async Task<IActionResult> GetUnpaidMonthsByEnrollment([FromRoute] int enrollment_id)
        {
            var result = await _studentMonthlyFeeRepository.GetUnpaidMonthsByEnrollmentAsync(enrollment_id);
            if(!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
