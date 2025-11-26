namespace StudentManagementSystem.Models.AttendanceDtos
{
    public class TakeAttendancesDto
    {
        public int Routine_id { get; set; }
        public DateOnly Session_date { get; set; }
        public List<AttendanceInput>? AttendanceInputs { get; set; }
    }

    public class AttendanceInput 
    { 
        public int? Enrollment_id { get; set; }
        public string? Status { get; set; }
        //public string? Remarks { get; set; } = null;

        
    }
}
