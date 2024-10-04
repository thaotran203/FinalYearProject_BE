using AutoMapper;
using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository;
using FinalYearProject_BE.Repository.IRepository;
using FinalYearProject_BE.Services.IService;

namespace FinalYearProject_BE.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService (ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task CreateCategory(CategoryDTO categoryDto)
        {
            if (await _categoryRepository.ExistsByName(categoryDto.Name))
            {
                throw new InvalidOperationException("Category already exists!");
            }
            var category = _mapper.Map<CategoryModel>(categoryDto);
            await _categoryRepository.CreateCategory(category);
        }

        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task UpdateCategory(int id, CategoryDTO categoryDto)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            
            category.Name = categoryDto.Name;
            await _categoryRepository.UpdateCategory(category);
        }

        public async Task SoftDeleteCategory(int id)
        {
            await _categoryRepository.SoftDeleteCategory(id);
        }

        public async Task RestoreCategory(int id)
        {
            await _categoryRepository.RestoreCategory(id);
        }

        public async Task HardDeleteCategory(int id)
        {
            await _categoryRepository.HardDeleteCategory(id);
        }
    }
}
