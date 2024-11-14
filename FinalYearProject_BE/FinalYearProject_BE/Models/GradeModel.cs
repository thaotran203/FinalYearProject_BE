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
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public double Grade { get; set; }

        [Required]
        [ForeignKey("Test")]
        public int TestId { get; set; }
        [ValidateNever]
        public TestModel Test { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ValidateNever]
        public UserModel User { get; set; }
    }
}
