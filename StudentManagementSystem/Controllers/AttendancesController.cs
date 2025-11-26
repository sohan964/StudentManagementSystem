using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.AttendanceDtos;
using StudentManagementSystem.Repositories.AttendanceRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendancesController(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        [HttpPost("take-attendance")]
        public async Task<IActionResult> TakeAttendance([FromBody] TakeAttendancesDto takeAttendancesDto)
        {
            var result = await _attendanceRepository.TakeAttendaceAsync(takeAttendancesDto);
            if(!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
