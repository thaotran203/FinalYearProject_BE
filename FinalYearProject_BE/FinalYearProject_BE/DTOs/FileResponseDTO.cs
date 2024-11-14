namespace FinalYearProject_BE.DTOs
{
    public class FileResponseDTO
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public string? FileType { get; set; }
        public int LessonId { get; set; }
    }
}
