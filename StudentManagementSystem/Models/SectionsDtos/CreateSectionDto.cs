namespace StudentManagementSystem.Models.SectionsDtos
{
    public class CreateSectionDto
    {
        public int Class_id { get; set; }
        public string? Section_name { get; set; }
        public int Capacity { get; set; }

    }
}
