using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalYearProject_BE.Models
{
    [Table("Test")]
    public class TestModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TimeLimit { get; set; }

        [Required]
        public int TotalQuestion { get; set; }

        [Required]
        [ForeignKey("Lesson")]
        public int LessonId { get; set; }
        [ValidateNever]
        public LessonModel Lesson { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<TestQuestionModel> TestQuestions { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<GradeModel> Grades { get; set; }
    }
}
