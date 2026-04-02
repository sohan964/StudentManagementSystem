namespace StudentManagementSystem.Models.NoticeDtos
{
    public class UpdateNoticeDto
    {
        public string Notice_title { get; set; } = string.Empty;

        public string Notice_description { get; set; } = string.Empty;

        public DateTime Notice_date { get; set; }

        public DateTime? Expiry_date { get; set; }
    }
}
