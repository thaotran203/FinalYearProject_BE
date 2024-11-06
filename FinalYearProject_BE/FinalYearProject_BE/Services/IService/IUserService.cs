﻿using FinalYearProject_BE.DTOs;

namespace FinalYearProject_BE.Services.IService
{
    public interface IUserService
    {
        Task RegisterStudent(RegisterUserDTO registerDto);
        Task<LoginResponseDTO> Login(string email, string password);
        Task CreateUserForTeacher(RegisterUserDTO registerDto);
        Task<List<UserDTO>> GetAllUsers(string? searchString, int? roleId);
        Task<UserDTO> GetUserById(int id);
        Task UpdateUser(int id, UpdateUserDTO updateDto);
        Task SoftDeleteUser(int id);
        Task HardDeleteUser(int id);
        Task RestoreUser(int id);
        Task RequestPasswordReset(string email);
        Task ResetPassword(ResetPasswordDTO resetPasswordDto);
        Task UpdatePassword(int userId, UpdatePasswordDTO updatePasswordDto);
        Task<LoginResponseDTO> RefreshJwtToken(string refreshToken);
        Task Logout(string refreshToken);
    }
}