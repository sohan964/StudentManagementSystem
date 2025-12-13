namespace StudentManagementSystem.Models.ClassRoutineDtos
{
    public class TeacherRoutineDto
    {
        public int? Routine_id { get; set; }
        public string? Day_name { get; set; }
        public int? Slot_number { get; set; }
        public string? Start_time { get; set; }
        public string? End_time { get; set;}
        public string? Subject_name { get; set; }
        public string? Class_name { get; set; }
        public string? Section_name { get; set; }
        public int? Year_id { get; set; }
        public int? Class_id { get; set; }
        public int? Section_id { get; set; }
        public int? Subject_id { get; set; }
    }
}
