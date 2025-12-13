namespace StudentManagementSystem.Models.AttendanceDtos
{
    public class StudentSubjectAttendance
    {
        public int? Record_id { get; set; }
        public int? Session_id { get; set; }
        public DateOnly? Session_date { get; set; }
        public string? Status { get; set; }
       
    }
}
