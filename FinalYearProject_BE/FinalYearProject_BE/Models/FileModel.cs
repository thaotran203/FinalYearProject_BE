using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalYearProject_BE.Models
{
    [Table("File")]
    public class FileModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? FileUrl { get; set; }

        public string? ImageUrl { get; set; }

        public string? VideoUrl { get; set; }

        [Required]
        [ForeignKey("Lesson")]
        public int LessonId { get; set; }
        [ValidateNever]
        public LessonModel Lesson { get; set; }

    }
}
