using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalYearProject_BE.Models
{
    [Table("Lesson")]
    public class LessonModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        [ValidateNever]
        public CourseModel Course { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<QuestionModel> Questions { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<TestModel> Tests { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<FileModel> Files { get; set; }
    }
}
