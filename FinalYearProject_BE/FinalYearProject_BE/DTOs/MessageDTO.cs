namespace FinalYearProject_BE.DTOs
{
    public class MessageDTO
    {
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }
        public int CourseId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
