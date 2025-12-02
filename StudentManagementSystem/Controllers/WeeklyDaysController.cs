using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Repositories.WeeklyDaysRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeeklyDaysController : ControllerBase
    {
        private readonly IWeeklyDaysRepository _weeklyDaysRepository;

        public WeeklyDaysController(IWeeklyDaysRepository weeklyDaysRepository)
        {
            _weeklyDaysRepository = weeklyDaysRepository;
        }
        [HttpGet("get-days")]
        public async Task<IActionResult> GetWeeklyDays()
        {
            var result = await _weeklyDaysRepository.GetWeeklyDaysAsync();
            return Ok(result);
        }

        [HttpGet("get-slots")]
        public async Task<IActionResult> GetTimeSlots()
        {
            var result = await _weeklyDaysRepository.GetTimeSlotsAsync();
            return Ok(result);
        }
    }
}
