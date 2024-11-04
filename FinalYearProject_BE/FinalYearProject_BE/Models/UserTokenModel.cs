using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalYearProject_BE.Models
{
    [Table("UserToken")]
    public class UserTokenModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime Expiration { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ValidateNever]
        public UserModel User { get; set; }

        [Required]
        public TokenType TokenType { get; set; }
    }

    public enum TokenType
    {
        ResetPassword = 1,
        Refresh = 2
    }
}
