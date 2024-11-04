using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalYearProject_BE.Models
{
    [Table("Course")]
    public class CourseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string ImageLink { get; set; }

        public int InstructorId { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public CategoryModel Category { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<LessonModel> Lessons { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<EnrollmentModel> Enrollments { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<PaymentModel> Payments { get; set; }

    }
}
