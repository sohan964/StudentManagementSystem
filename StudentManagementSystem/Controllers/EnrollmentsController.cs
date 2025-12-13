using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.EnrollmentsDtos;
using StudentManagementSystem.Repositories.EnrollmentsRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentsRepository _enrollmentsRepository;

        public EnrollmentsController(IEnrollmentsRepository enrollmentsRepository)
        {
            _enrollmentsRepository = enrollmentsRepository;
        }

        [HttpPost("new-enrollment")]
        public async Task<IActionResult> AddEnrollment([FromBody] AddEnrollmentDto addEnrollmentDto)
        {
            var result = await _enrollmentsRepository.AddEnrollmentAsync(addEnrollmentDto);
            return Ok(result);
        }

        [HttpGet("get-enrollments")]
        public async Task<IActionResult> GetEnrollmentsByParameters([FromQuery] int? year_id, [FromQuery] int? class_id, [FromQuery] int? section_id,[FromQuery] string? status)
        {
            var result = await _enrollmentsRepository.GetEnrollmentsByParameterAsync(year_id, class_id, section_id, status);
            return Ok(result);
        }
    }
}
