using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Services;
using FinalYearProject_BE.Services.IService;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace FinalYearProject_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ICloudinaryService _cloudinaryService;

        public UserController(IUserService userService, IJwtTokenService jwtTokenService, ICloudinaryService cloudinaryService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterStudent(RegisterUserDTO registerUserDTO)
        {
            try
            {
                await _userService.RegisterStudent(registerUserDTO);
                return Ok(new { message = "Registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var token = await _userService.Login(email, password);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("Register/teacher")]
        public async Task<IActionResult> RegisterTeacher(RegisterUserDTO registerUserDTO)
        {
            try
            {
                await _userService.CreateUserForTeacher(registerUserDTO);
                return Ok(new { message = "Teacher registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(string? searchString, int? roleId)
        {
            var users = await _userService.GetAllUsers(searchString, roleId);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = int.Parse(User.FindFirst("Id")?.Value);
            var user = await _userService.GetUserProfile(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDTO updateUserDTO)
        {
            try
            {
                await _userService.UpdateUser(id, updateUserDTO);
                return Ok(new { message = "User updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("UploadAvatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var updatedUser = await _cloudinaryService.UploadUserImage(file);

            var userId = int.Parse(User.FindFirst("Id")?.Value);
            if (userId == null)
                return Unauthorized("User ID not found in token.");

            await _userService.UpdateUser(userId, updatedUser);

            return Ok(new { updatedUser.ImageUrl });
        }

        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            try
            {
                await _userService.SoftDeleteUser(id);
                return Ok(new { message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreUser(int id)
        {
            try
            {
                await _userService.RestoreUser(id);
                return Ok(new { message = "User restored successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("HardDelete/{id}")]
        public async Task<IActionResult> HardDeleteUser(int id)
        {
            try
            {
                await _userService.HardDeleteUser(id);
                return Ok(new { message = "User was permanently deleted." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("ChangePassword/{id}")]
        public async Task<IActionResult> ChangePassword(int id, UpdatePasswordDTO updatePasswordDTO)
        {
            try
            {
                await _userService.UpdatePassword(id, updatePasswordDTO);
                return Ok(new { message = "Password updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("RequestResetPassword")]
        public async Task<IActionResult> RequestPasswordReset(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is required.");
            }

            try
            {
                await _userService.RequestPasswordReset(email);
                return Ok("Password reset link was sent to your email");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDto)
        {
            if (resetPasswordDto == null ||
                string.IsNullOrEmpty(resetPasswordDto.Token) ||
                string.IsNullOrEmpty(resetPasswordDto.NewPassword) ||
                string.IsNullOrEmpty(resetPasswordDto.ConfirmPassword))
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                await _userService.ResetPassword(resetPasswordDto);
                return Ok("Password has been reset successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RefreshJwt")]
        public async Task<IActionResult> RefreshJwtToken(string refreshToken)
        {
            var token = await _userService.RefreshJwtToken(refreshToken);
            return Ok(new { token });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            await _userService.Logout(refreshToken);
            return Ok("Logged out successfully.");
        }
    }
}
