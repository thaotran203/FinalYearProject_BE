using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalYearProject_BE.Models
{
    [Table("Message")]
    public class MessageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string MessageContent { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        [Required]
        [ForeignKey("Sender")]
        public int SenderId { get; set; }
        [ValidateNever]
        public UserModel Sender { get; set; }

        [Required]
        [ForeignKey("Receiver")]
        public int ReceiverId { get; set; }
        [ValidateNever]
        public UserModel Receiver { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        [ValidateNever]
        public CourseModel Course { get; set; }
    }
}
