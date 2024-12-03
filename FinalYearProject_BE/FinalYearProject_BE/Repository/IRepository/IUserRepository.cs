using FinalYearProject_BE.Models;

namespace FinalYearProject_BE.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<UserModel> GetUserById(int id);
        Task<List<UserModel>> GetAllUsers(string searchString, int? roleId);
        Task<UserModel> GetUserByEmail(string email);
        Task<List<UserModel>> GetUsersByRole(string roleName);
        Task<UserModel> GetUserProfile(int id);
        Task CreateUser(UserModel user);
        Task UpdateUser(UserModel user);
        Task SoftDeleteUser(int id);
        Task RestoreUser(int id);
        Task HardDeleteUser(int id);
    }
}
