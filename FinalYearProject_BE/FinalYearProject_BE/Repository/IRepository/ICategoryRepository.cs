using FinalYearProject_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject_BE.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task<List<CategoryModel>> GetCategories();
        Task<CategoryModel> GetCategoryById(int id);
        Task CreateCategory(CategoryModel category);
        Task UpdateCategory(CategoryModel category);
        Task DeleteCategory(CategoryModel category);
        Task<bool> ExistsByName(string name);
    }
}
