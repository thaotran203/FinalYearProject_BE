using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalYearProject_BE.Models
{
    [Table("TestQuestion")]
    public class TestQuestionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Test")]
        public int TestId { get; set; }
        [ValidateNever]
        public TestModel Test { get; set; }

        [Required]
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        [ValidateNever]
        public QuestionModel Question { get; set; }

    }
}
