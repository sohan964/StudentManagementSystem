namespace StudentManagementSystem.Models.TeachersDtos
{
    public class TeachersDto
    {
        public int Teacher_id { get; set; }
        public string? User_id { get; set; }
        public string? Teacher_code { get; set; }
        public string? First_name { get; set; }
        public string? Last_name { get; set; }
        public int Department_id { get; set; }
        public string? Contact { get; set; }
        public DateOnly? Hire_date { get; set; }
        public string? Photo { get; set; }
    }
}
