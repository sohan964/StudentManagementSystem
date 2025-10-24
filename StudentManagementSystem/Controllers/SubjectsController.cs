using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.SubjectDtos;
using StudentManagementSystem.Repositories.SubjectsRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectsRepository _subjectsRepository;

        public SubjectsController(ISubjectsRepository subjectsRepository)
        {
            _subjectsRepository = subjectsRepository;
        }

        [HttpGet("get-subjects")]
        public async Task<IActionResult> GetSubjects()
        {
            var result = await _subjectsRepository.GetSubjectsAsync();
            return Ok(result);
        }

        [HttpGet("get-subject/{id}")]
        public async Task<IActionResult> GetSubjectById([FromRoute] int id)
        {
            var result = await _subjectsRepository.GetSubjectByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("add-subject")]
        public async Task<IActionResult> AddSubject(CreateSubjectDto createSubjectDto)
        {
            var result = await _subjectsRepository.AddSubjectAsync(createSubjectDto);
            return Ok(result);
        }
    }
}
