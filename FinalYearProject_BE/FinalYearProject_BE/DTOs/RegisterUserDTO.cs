using System.ComponentModel.DataAnnotations;

namespace FinalYearProject_BE.DTOs
{
    public class RegisterUserDTO
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
