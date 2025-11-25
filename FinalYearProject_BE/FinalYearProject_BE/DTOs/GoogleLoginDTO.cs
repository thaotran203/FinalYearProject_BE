using System.ComponentModel.DataAnnotations;

namespace FinalYearProject_BE.DTOs
{
    public class GoogleLoginDTO
    {
        [Required]
        public string IdToken { get; set; }
    }
}
