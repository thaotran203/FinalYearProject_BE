using System.ComponentModel.DataAnnotations;

namespace FinalYearProject_BE.DTOs
{
    public class LessonDTO
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        [Required]
        public int CourseId { get; set; }
    }

}
