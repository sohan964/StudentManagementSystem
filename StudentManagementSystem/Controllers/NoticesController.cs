using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.NoticeDtos;
using StudentManagementSystem.Repositories.NoticeRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticesController : ControllerBase
    {
        private readonly INoticeRepository _noticeRepository;

        public NoticesController(INoticeRepository noticeRepository)
        {
            _noticeRepository = noticeRepository;
        }

        [HttpGet("get-all-notices")]
        public async Task<IActionResult> GetAllNotices()
        {
            var result = await _noticeRepository.GetNoticesAsync();
            if(!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpPost("add-notice")]
        public async Task<IActionResult> AddNotice([FromBody] AddNoticeDto notice)
        {
            var result = await _noticeRepository.AddNoticesAsync(notice);
            if(!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
