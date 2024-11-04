using FinalYearProject_BE.Models;

namespace FinalYearProject_BE.Repository.IRepository
{
    public interface IUserTokenRepository
    {
        Task<UserTokenModel> GetTokenByUserId(int userId, TokenType tokenType);
        Task<UserTokenModel> GetTokenByToken(string token);
        Task<UserTokenModel> GetRefreshToken(string token);
        Task AddToken(UserTokenModel token);
        Task DeleteToken(UserTokenModel token);
    }
}
