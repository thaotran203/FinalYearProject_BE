using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalYearProject_BE.Models
{
    [Table("Question")]
    public class QuestionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [ForeignKey("FinalTest")]
        public int FinalTestId { get; set; }
        [ValidateNever]
        public FinalTestModel FinalTest { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<AnswerModel> Answers { get; set; }
    }
}
