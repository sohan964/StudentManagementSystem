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
    }
}
