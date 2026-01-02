namespace StudentManagementSystem.Models.NoticeDtos
{
    public class AddNoticeDto
    {
        public string? Notice_title { get; set; }
        public string? Notice_description { get; set; }
        //public DateOnly? Notice_date { get; set; } = new DateOnly();
        public DateOnly? Expiry_date { get; set; }

    }
}
