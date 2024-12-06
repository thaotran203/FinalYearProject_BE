using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalYearProject_BE.Models
{
    [Table("FinalTest")]
    public class FinalTestModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TimeLimit { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        [ValidateNever]
        public CourseModel Course { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<QuestionModel> Questions { get; set; }
    }
}
