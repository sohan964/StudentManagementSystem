using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.ResultsDtos;
using StudentManagementSystem.Repositories.ResultsRepositories;
using System.Collections.Generic;

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

        [HttpGet("get-final-result/{enrollment_id}")]
        public async Task<IActionResult> GetFinalResultByEnrollment([FromRoute]int? enrollment_id)
        {
            var result = await _resultsRepository.GetFinalResultByEnrollmentAsync(enrollment_id);
            return Ok(result);
        }

        [HttpGet("get-subjects-results/{enrollment_id}")]
        public async Task<IActionResult> GetAllSubjectsTotalResults([FromRoute] int enrollment_id)
        {
            var result = await _resultsRepository.GetAllSubjectsTotalResultAsync(enrollment_id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpGet("get-details-result/{enrollment_id}/{subject_id}")]
        public async Task<IActionResult> GetSubjectDetailResults([FromRoute] int enrollment_id, [FromRoute] int subject_id)
        {
            var result = await _resultsRepository.GetSubjectDetailResultsAsync(enrollment_id, subject_id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
