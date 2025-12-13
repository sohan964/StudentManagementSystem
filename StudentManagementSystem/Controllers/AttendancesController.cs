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

        [HttpGet("attendance-summary")]
        public async Task<IActionResult> GetAttendanceSummary([FromQuery]int year_id, [FromQuery] int class_id, [FromQuery] int section_id, [FromQuery] int subject_id)
        {
            var result = await _attendanceRepository.GetAttendanceSummaryAsync(year_id, class_id, section_id, subject_id);
            return Ok(result);
        }

        [HttpGet("attendance-details")]
        public async Task<IActionResult> GetAttendanceDetails([FromQuery] int enrollment_id, [FromQuery] int year_id, [FromQuery] int subject_id)
        {
            var result = await _attendanceRepository.GetAttendanceDetailsAsync(enrollment_id, year_id, subject_id);
            return Ok(result);
        }
        [HttpPut("update-attendance/{record_id}/{status}")]
        public async Task<IActionResult> UpdateAttendanceByRecord([FromRoute]int record_id, [FromRoute] string status)
        {
            var result = await _attendanceRepository.UpdateAttendanceByRecordAsync(record_id, status);
            return Ok(result);
        }
    }
}
