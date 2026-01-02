using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.NoticeDtos;

namespace StudentManagementSystem.Repositories.NoticeRepositories
{
    public interface INoticeRepository
    {
        Task<Response<List<GetNoticesDto>>> GetNoticesAsync();
        Task<Response<object>> AddNoticesAsync(AddNoticeDto addNotice);
    }
}
