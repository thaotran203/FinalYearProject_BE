namespace FinalYearProject_BE.DTOs
{
    public class SendMessageDTO
    {
        public int CourseId { get; set; }
        public string MessageContent { get; set; }
        public int? StudentId { get; set; }
    }
}
