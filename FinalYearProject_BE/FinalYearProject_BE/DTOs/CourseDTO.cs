namespace FinalYearProject_BE.DTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CourseContent { get; set; }
        public IFormFile Image { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public int InstructorId { get; set; }
    }
}
