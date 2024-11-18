namespace FinalYearProject_BE.DTOs
{
    public class VideoUploadDTO
    {
        public IFormFile Video { get; set; }
        public int LessonId { get; set; }
    }
}
