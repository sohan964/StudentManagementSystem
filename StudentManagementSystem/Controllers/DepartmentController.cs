using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.DepartmentDtos;
using StudentManagementSystem.Repositories.DepartmentRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet("getdepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            var result = await _departmentRepository.GetDepartmentsAsync();
            return Ok(result);
        }

        [HttpGet("get-departmentbyid/{id}")]
        public async Task<IActionResult> GetDepartmentById([FromRoute] int id)
        {
            var result = await _departmentRepository.GetDepartmentByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("adddepartment")]
        public async Task<IActionResult> AddDepartment([FromBody]CreateDepartmentDto createDepartmentDto)
        {
            var result = await _departmentRepository.AddDepartmentAsync(createDepartmentDto);
            return Ok(result);
        }
        [HttpPut("update-department/{id}")]
        public async Task<IActionResult> UpdateDepartment([FromBody]DepartmentDto departmentDto, [FromRoute]int id)
        {
            departmentDto.Department_id = id;
            var result = await _departmentRepository.UpdateDepartmentAsync(departmentDto);
            return Ok(result);
        }
    }
}
