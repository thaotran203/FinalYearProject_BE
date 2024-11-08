using AutoMapper;
using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using FinalYearProject_BE.Services.IService;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using NuGet.Common;

namespace FinalYearProject_BE.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<UserModel> _passwordHasher;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IEmailService _emailService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher<UserModel> passwordHasher,
            IUserTokenRepository userTokenRepository, IEmailService emailService, IJwtTokenService jwtTokenService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _userTokenRepository = userTokenRepository;
            _emailService = emailService;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
        }

        public async Task RegisterStudent(RegisterUserDTO registerDto)
        {
            var existedUser = await _userRepository.GetUserByEmail(registerDto.Email);
            if (existedUser != null)
            {
                throw new Exception("Email already exists.");
            }

            var newUser = _mapper.Map<UserModel>(registerDto);
            newUser.RoleId = 1;
            newUser.IsDeleted = false;
            newUser.Password = _passwordHasher.HashPassword(newUser, registerDto.Password);

            await _userRepository.CreateUser(newUser);
        }

        public async Task<LoginResponseDTO> Login(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Email doesn't exist.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result != PasswordVerificationResult.Success)
            {
                throw new UnauthorizedAccessException("Wrong password! Please enter again.");
            }

            var jwtToken = _jwtTokenService.GenerateToken(user);
            var refreshToken = Guid.NewGuid().ToString();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(30);

            var userToken = new UserTokenModel
            {
                Token = refreshToken,
                Expiration = refreshTokenExpiration,
                UserId = user.Id,
                TokenType = TokenType.Refresh
            };

            await _userTokenRepository.AddToken(userToken);
            return new LoginResponseDTO
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = refreshTokenExpiration
            };
        }

        public async Task CreateUserForTeacher(RegisterUserDTO registerDto)
        {
            var existedUser = await _userRepository.GetUserByEmail(registerDto.Email);
            if (existedUser != null)
            {
                throw new Exception("Email already exists.");
            }

            var newUser = _mapper.Map<UserModel>(registerDto);
            newUser.RoleId = 2;
            newUser.IsDeleted = false;
            newUser.Password = _passwordHasher.HashPassword(newUser, registerDto.Password);

            await _emailService.SendRegistrationEmail(registerDto.Email, registerDto.Password);
            await _userRepository.CreateUser(newUser);
        }

        public async Task<List<UserDTO>> GetAllUsers(string? searchString, int? roleId)
        {
            var users = await _userRepository.GetAllUsers(searchString, roleId);
            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetUserProfile(int id)
        {
            var user = await _userRepository.GetUserById(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task UpdateUser(int id, UpdateUserDTO updateDto)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.FullName = updateDto.FullName ?? user.FullName;
            user.Email = updateDto.Email ?? user.Email;
            user.PhoneNumber = updateDto.PhoneNumber ?? user.PhoneNumber;
            user.ImageUrl = updateDto.ImageUrl ?? user.ImageUrl;

            await _userRepository.UpdateUser(user);
        }

        public async Task UpdatePassword(int userId, UpdatePasswordDTO updatePasswordDto)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
                throw new Exception("User not found");

            var passwordVerification = _passwordHasher.VerifyHashedPassword(user, user.Password, updatePasswordDto.CurrentPassword);
            if (passwordVerification == PasswordVerificationResult.Failed)
            {
                throw new Exception("Current password is incorrect");
            }

            if (updatePasswordDto.NewPassword != updatePasswordDto.ConfirmPassword)
            {
                throw new Exception("New password and confirm password do not match");
            }

            user.Password = _passwordHasher.HashPassword(user, updatePasswordDto.NewPassword);
            await _userRepository.UpdateUser(user);
        }

        public async Task SoftDeleteUser(int id)
        {
            await _userRepository.SoftDeleteUser(id);
        }

        public async Task HardDeleteUser(int id)
        {
            await _userRepository.HardDeleteUser(id);
        }

        public async Task RestoreUser(int id)
        {
            await _userRepository.RestoreUser(id);
        }

        public async Task RequestResetPassword(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null || user.IsDeleted)
            {
                throw new Exception("User not found.");
            }

            var existingToken = await _userTokenRepository.GetTokenByUserId(user.Id, TokenType.ResetPassword);
            if (existingToken != null)
            {
                await _userTokenRepository.DeleteToken(existingToken);
            }

            var resetToken = Guid.NewGuid().ToString();
            var tokenExpiration = DateTime.UtcNow.AddHours(1);

            var userToken = new UserTokenModel
            {
                Token = resetToken,
                Expiration = tokenExpiration,
                UserId = user.Id,
                TokenType = TokenType.ResetPassword
            };

            await _userTokenRepository.AddToken(userToken);

            var resetLink = $"{_configuration["AppUrl"]}/reset-password?token={resetToken}";
            await _emailService.SendPasswordResetEmail(user.Email, resetLink);
        }

        public async Task ResetPassword(ResetPasswordDTO resetPasswordDto)
        {
            var userToken = await _userTokenRepository.GetTokenByToken(resetPasswordDto.Token);

            if (userToken == null)
            {
                throw new ArgumentException("Invalid reset password token.");
            }
            if (userToken.Expiration < DateTime.UtcNow)
            {
                await _userTokenRepository.DeleteToken(userToken);
                throw new ArgumentException("Reset password token expired.");
            }

            var user = await _userRepository.GetUserById(userToken.UserId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.Password = _passwordHasher.HashPassword(user, resetPasswordDto.NewPassword);
            await _userRepository.UpdateUser(user);

            await _userTokenRepository.DeleteToken(userToken);
        }

        public async Task<LoginResponseDTO> RefreshJwtToken(string refreshToken)
        {
            var existingToken = await _userTokenRepository.GetRefreshToken(refreshToken);

            if (existingToken == null)
            {
                throw new ArgumentException("Invalid refresh token.");
            }

            if (existingToken.Expiration <= DateTime.UtcNow)
            {
                throw new ArgumentException("Refresh token expired.");
            }

            var user = await _userRepository.GetUserById(existingToken.UserId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var newJwtToken = _jwtTokenService.GenerateToken(user);

            return new LoginResponseDTO
            {
                JwtToken = newJwtToken,
                RefreshToken = existingToken.Token
            };
        }

        public async Task Logout(string refreshToken)
        {
            var existingToken = await _userTokenRepository.GetRefreshToken(refreshToken);

            if (existingToken != null)
            {
                await _userTokenRepository.DeleteToken(existingToken);
            }
        }
    }
}
