using FinalYearProject_BE.Models;

namespace FinalYearProject_BE.Services.IService
{
    public interface IJwtTokenService
    {
        string GenerateToken(UserModel user);
    }
}
