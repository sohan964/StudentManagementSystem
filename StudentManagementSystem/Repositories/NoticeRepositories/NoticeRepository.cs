using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.NoticeDtos;
using System.Data;
namespace StudentManagementSystem.Repositories.NoticeRepositories
{
    public class NoticeRepository(IConfiguration _configuration) : INoticeRepository
    {
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

        //get
        public async Task<Response<List<GetNoticesDto>>> GetNoticesAsync()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetAllNotices", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var noticesList = new List<GetNoticesDto>();
            while(await  reader.ReadAsync())
            {
                noticesList.Add(new GetNoticesDto()
                {
                    Notice_id = reader.GetInt32(0),
                    Notice_title = reader.GetString(1),
                    Notice_description = reader.GetString(2),
                    Notice_date = DateOnly.FromDateTime(reader.GetDateTime(3)),
                    Expiry_date = reader.IsDBNull(4) ? null : DateOnly.FromDateTime(reader.GetDateTime(4)),

                });
            }
            if (noticesList.Count == 0) return new Response<List<GetNoticesDto>>(false, "No NoticesFound");
            return new Response<List<GetNoticesDto>>(true, "All Notices", noticesList);
        }

        //add new notices
        public async Task<Response<object>> AddNoticesAsync( AddNoticeDto addNotice)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spAddNotice", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@notice_title", addNotice.Notice_title);
            command.Parameters.AddWithValue("@notice_description", addNotice.Notice_description);
            command.Parameters.AddWithValue("@notice_date", DateOnly.FromDateTime(DateTime.Now));
            command.Parameters.AddWithValue("@expiry_date", addNotice.Expiry_date);
            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            if (result == 0) return new Response<object>(false, "failed to add Notice");
            return new Response<object>(true, "Notice Added successfully");

        }

        public async Task<Response<object>> UpdateNoticeAsync(int notice_id, UpdateNoticeDto updateNotice)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spUpdateNotice", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            command.Parameters.AddWithValue("@notice_id", notice_id);
            command.Parameters.AddWithValue("@notice_title", updateNotice.Notice_title);
            command.Parameters.AddWithValue("@notice_description", updateNotice.Notice_description);
            command.Parameters.AddWithValue("@notice_date", updateNotice.Notice_date);
            command.Parameters.AddWithValue("@expiry_date",
                updateNotice.Expiry_date.HasValue
                    ? updateNotice.Expiry_date.Value
                    : DBNull.Value);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            string status = "error";
            string message = "Something went wrong";

            if (await reader.ReadAsync())
            {
                status = reader["status"]?.ToString() ?? "error";
                message = reader["message"]?.ToString() ?? message;
            }

            return new Response<object>(
                status == "success",
                message
            );
        }
    }
}
