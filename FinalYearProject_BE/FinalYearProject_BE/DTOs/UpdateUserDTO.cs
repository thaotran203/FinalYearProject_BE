using System.ComponentModel.DataAnnotations;

namespace FinalYearProject_BE.DTOs
{
    public class UpdateUserDTO
    {
        public string? FullName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? ImageUrl { get; set; }

    }
}
