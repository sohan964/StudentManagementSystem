using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.SectionsDtos;
using StudentManagementSystem.Repositories.SectionsRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly ISectionsRepository _sectionsRepository;

        public SectionsController(ISectionsRepository sectionsRepository)
        {
            _sectionsRepository = sectionsRepository;
        }

        [HttpPost("add-section")]
        public async Task<IActionResult> AddSection(CreateSectionDto createSectionDto)
        {
            var result = await _sectionsRepository.AddSectionAsync(createSectionDto);
            return Ok(result);
        }

        [HttpGet("get-sections")]
        public async Task<IActionResult> GetSections()
        {
            var result = await _sectionsRepository.GetSectionsAsync();
            return Ok(result);
        }

        
    }
}
