using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.NoticeDtos;

namespace StudentManagementSystem.Repositories.NoticeRepositories
{
    public interface INoticeRepository
    {
        Task<Response<List<GetNoticesDto>>> GetNoticesAsync();
        Task<Response<object>> AddNoticesAsync(AddNoticeDto addNotice);
        Task<Response<object>> UpdateNoticeAsync(int notice_id, UpdateNoticeDto updateNotice);
    }
}
