using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalYearProject_BE.Models
{
    [Table("LessonVideo")]
    public class LessonVideoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string VideoUrl { get; set; }

        public string? Transcript { get; set; }

        [StringLength(50)]
        public string ProcessingStatus { get; set; } = "Pending";

        public string? ErrorMessage { get; set; }

        [Required]
        [ForeignKey("Lesson")]
        public int LessonId { get; set; }
        [ValidateNever]
        public LessonModel Lesson { get; set; }

    }
}
