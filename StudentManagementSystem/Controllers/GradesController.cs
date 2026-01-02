using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Repositories.GradeRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly IGradesRepository _gradesRepository;

        public GradesController(IGradesRepository gradesRepository)
        {
            _gradesRepository = gradesRepository;
        }
        [HttpGet("get-grade-list")]
        public async Task<IActionResult> GetGradeList()
        {
            var result = await _gradesRepository.GetGradeListAsync();
            if(result.Success == false) return NotFound(result);
            return Ok(result);
        }
    }
}
