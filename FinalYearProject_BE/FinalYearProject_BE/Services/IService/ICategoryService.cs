using FinalYearProject_BE.DTOs;

namespace FinalYearProject_BE.Services.IService
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetCategories();
        Task<CategoryDTO> GetCategoryById(int id);
        Task CreateCategory(CategoryDTO categoryDto);
        Task UpdateCategory(int id, CategoryDTO categoryDto);
        Task DeleteCategory(int id);
    }
}
