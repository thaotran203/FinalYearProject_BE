using System.ComponentModel.DataAnnotations;

namespace FinalYearProject_BE.DTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Course description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Course price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Course price must be a positive number.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Category id is required.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Instructor id is required.")]
        public int InstructorId { get; set; }
    }
}
