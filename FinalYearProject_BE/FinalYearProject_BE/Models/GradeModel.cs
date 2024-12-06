using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalYearProject_BE.Models
{
    [Table("Grade")]
    public class GradeModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime TestDate { get; set; }

        [Required]
        public double Grade { get; set; }

        [Required]
        [ForeignKey("FinalTest")]
        public int FinalTestId { get; set; }
        [ValidateNever]
        public FinalTestModel FinalTest { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ValidateNever]
        public UserModel User { get; set; }
    }
}
