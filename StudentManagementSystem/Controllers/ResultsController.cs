using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.ResultsDtos;
using StudentManagementSystem.Repositories.ResultsRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly IResultsRepository _resultsRepository;

        public ResultsController(IResultsRepository resultsRepository)
        {
            _resultsRepository = resultsRepository;
        }

        [HttpPost("add-result")]
        public async Task<IActionResult> AddResultS([FromBody] AddResultDto addResultDto)
        {
            var result = await _resultsRepository.AddResultsAsync(addResultDto);
            return Ok(result);
        }

        [HttpPost("get-result/{exam_session_id}/{enrolloment_id}")]
        public async Task<IActionResult> GetResultByEnrollment([FromRoute] int? exam_session_id, [FromRoute] int? enrolloment_id)
        {
            var result = await _resultsRepository.GetResultBySessionAndEnrollmentAsync(exam_session_id, enrolloment_id);
            if(!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
