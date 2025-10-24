using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.ClassesDtos;
using StudentManagementSystem.Repositories.ClassesRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassesRepository _classesRepository;

        public ClassesController(IClassesRepository classesRepository)
        {
            _classesRepository = classesRepository;
        }

        [HttpGet("get-classes")]
        public async Task<IActionResult> GetClasses()
        {
            var result = await _classesRepository.GetClassesAsync();
            return Ok(result);
        }

        [HttpGet("get-class/{class_id}")]
        public async Task<IActionResult> GetClassById([FromRoute] int class_id)
        {
            var result = await _classesRepository.GetClassByIdAsync(class_id);
            return Ok(result);
        }

        ///handle ClassSubjects another table to 
        [HttpPost("add-class-subject")]
        public async Task<IActionResult> AddClassSubjects([FromBody] ClassSubjectsDto classSubjectsDto)
        {
            var result = await _classesRepository.AddClassSubjecsAsync(classSubjectsDto);
            return Ok(result);
        }


        [HttpGet("get-class-subjects/{class_id}")]
        public async Task<IActionResult> GetClassSubjectsByClassId([FromRoute] int class_id)
        {
            var result = await _classesRepository.GetSubjectsByClassIdAsync(class_id);
            return Ok(result);
        }
        // to be continued later ..........
    }
}
