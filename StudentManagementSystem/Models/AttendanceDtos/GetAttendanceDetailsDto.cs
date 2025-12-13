namespace StudentManagementSystem.Models.AttendanceDtos
{
    public class GetAttendanceDetailsDto
    {
        public int? Subject_id { get; set; }
        public int? Class_id { get; set; }
        public int? Section_id { get; set; }
        public int? Teacher_id { get; set; }
        public int? Student_id { get; set; }
        public string? Student_name { get; set; }
        public List<StudentSubjectAttendance>? studentSubjectAttendances { get; set; } = new List<StudentSubjectAttendance>();

    }
}
