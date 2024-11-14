using FinalYearProject_BE.Repository;
using FinalYearProject_BE.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;

namespace FinalYearProject_BE
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(ILogger<JwtMiddleware> logger, RequestDelegate next)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IUserRepository userRepository)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken != null)
                {
                    var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id");

                    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Invalid token: User ID is missing or invalid.");
                        return;
                    }

                    var tokenVersionInJwt = int.Parse(jwtToken.Claims.First(c => c.Type == "TokenVersion").Value);

                    var user = await userRepository.GetUserById(userId);

                    if (user == null || user.TokenVersion != tokenVersionInJwt)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Token is no longer valid.");
                        return;
                    }
                }
            }

            await _next(context);
        }

    }
}
