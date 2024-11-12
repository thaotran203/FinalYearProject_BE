namespace FinalYearProject_BE.DTOs
{
    public class FileUploadDTO
    {
        public string FileName { get; set; }
        public IFormFile File { get; set; }
        public string FileType { get; set; }
        public int LessonId { get; set; }
    }
}
