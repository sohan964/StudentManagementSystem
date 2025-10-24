using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.AcademicYearsDtos;
using StudentManagementSystem.Repositories.AcademicYearsRepository;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YearsController : ControllerBase
    {
        private readonly IYearsRepository _yearsRepository;

        public YearsController(IYearsRepository yearsRepository)
        {
            _yearsRepository = yearsRepository;
        }

        [HttpPost("Add-Years")]
        public async Task<IActionResult> AddAcademicYear(AddAcademicYearDto academicYearDto)
        {
            var result = await _yearsRepository.AddAcademicYearAsync(academicYearDto);
            return Ok(result);
        }

        [HttpGet("get-academic-years")]
        public async Task<IActionResult> GetAcademicYears()
        {
            var result = await _yearsRepository.GetAcademicYearsAsync();
            return Ok(result);
        }
    }
}
