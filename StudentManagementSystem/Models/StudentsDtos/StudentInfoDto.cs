namespace StudentManagementSystem.Models.StudentsDtos
{
    public class StudentInfoDto
    {
        public int? Student_id { get; set; }
        public string? Student_number { get; set; }
        public string? First_name { get; set; }
        public string? Last_name { get; set;}
        public DateOnly? DOB { get; set; }
        public string? Gender { get; set; }
        public string? Photo {  get; set; }
        public int? Admission_year { get; set; }
        public string? Address { get; set; }

        //class
        public int? Class_id { get; set; }
        public string? Class_name { get;set; }
        //section:
        public int? Section_id { get; set; }
        public string? Section_name { get; set; }

        //AspNetUsers
        public string? User_id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber {  get; set; }

    }
}
