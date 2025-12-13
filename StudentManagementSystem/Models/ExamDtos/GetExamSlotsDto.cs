namespace StudentManagementSystem.Models.ExamDtos
{
    public class GetExamSlotsDto
    {
        public int? Exam_slot_id { get; set; }
        public string? Exam_slot_name { get; set; }
        public string? Exam_start_time {  get; set; }
        public string? Exam_end_time { get; set; }
    }
}
