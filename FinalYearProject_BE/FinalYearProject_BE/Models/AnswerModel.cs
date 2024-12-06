using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalYearProject_BE.Models
{
    [Table("Answer")]
    public class AnswerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public bool IsCorrect { get; set; }

        [Required]
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        [ValidateNever]
        public QuestionModel Question { get; set; }
    }
}
