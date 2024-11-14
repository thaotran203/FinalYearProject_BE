using FinalYearProject_BE.DTOs;

namespace FinalYearProject_BE.Services.IService
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllCategories();
        Task<CategoryDTO> GetCategoryById(int id);
        Task CreateCategory(CategoryDTO categoryDto);
        Task UpdateCategory(int id, CategoryDTO categoryDto);
        Task SoftDeleteCategory(int id);
        Task RestoreCategory(int id);
        Task HardDeleteCategory(int id);
    }
}
