using FinalYearProject_BE.Data;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject_BE.Repository
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public UserTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddToken(UserTokenModel token)
        {
            _context.UserTokens.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task<UserTokenModel> GetTokenByUserId(int userId, TokenType tokenType)
        {
            return await _context.UserTokens
                                 .FirstOrDefaultAsync(t => t.UserId == userId && t.TokenType == tokenType);
        }

        public async Task<UserTokenModel> GetTokenByToken(string token)
        {
            return await _context.UserTokens
                                 .FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task<UserTokenModel> GetRefreshToken(string token)
        {
            return await _context.UserTokens
                                 .FirstOrDefaultAsync(t => t.Token == token && t.TokenType == TokenType.Refresh);
        }

        public async Task DeleteToken(UserTokenModel token)
        {
            _context.UserTokens.Remove(token);
            await _context.SaveChangesAsync();
        }
    }
}
