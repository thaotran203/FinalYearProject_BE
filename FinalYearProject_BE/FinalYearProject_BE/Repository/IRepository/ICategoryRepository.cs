using FinalYearProject_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject_BE.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task<List<CategoryModel>> GetAllCategories();
        Task<CategoryModel> GetCategoryById(int id);
        Task CreateCategory(CategoryModel category);
        Task UpdateCategory(CategoryModel category);
        Task SoftDeleteCategory(int id);
        Task RestoreCategory(int id);
        Task HardDeleteCategory(int id);
        Task<bool> ExistsByName(string name);
    }
}
