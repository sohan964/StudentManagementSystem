namespace StudentManagementSystem.Models.AttendanceDtos
{
    public class GetAttendancesDto
    {
        public int? Enrollment_id { get; set; }
        public int? Student_id { get; set; }
        public string? Student_number { get; set; }
        public int? Year_id { get; set; }
        public int? Class_id { get; set; }
        public int? Section_id { get; set; }
        public int? Subject_id { get; set; }
        public string? First_name { get; set; }
        public string? Last_name { get; set; }
        public int? Total_classes { get; set; }
        public int? Total_present {  get; set; }
        public int? Total_absent { get; set; }
        public int? Attendance_percentage { get; set; }

    }
}
