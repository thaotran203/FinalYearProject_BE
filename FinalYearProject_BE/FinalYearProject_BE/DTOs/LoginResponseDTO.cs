using System.ComponentModel.DataAnnotations;

namespace FinalYearProject_BE.DTOs
{
    public class LoginResponseDTO
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
