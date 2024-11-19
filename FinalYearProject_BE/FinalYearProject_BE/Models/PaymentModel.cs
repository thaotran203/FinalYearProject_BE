using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalYearProject_BE.Models
{
    [Table("Payment")]
    public class PaymentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ValidateNever]
        public UserModel User { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        [ValidateNever]
        public CourseModel Course { get; set; }
    }
}
