using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalYearProject_BE.Models
{
    [Table("User")]
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        [ValidateNever]
        public RoleModel Role { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<EnrollmentModel> Enrollments { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<PaymentModel> Payments { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<GradeModel> Grades { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<MessageModel> SentMessages { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<MessageModel> ReceivedMessages { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<LessonProgressModel> LessonProgresses { get; set; }

    }
}
