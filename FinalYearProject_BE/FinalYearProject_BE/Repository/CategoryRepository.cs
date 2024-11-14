using FinalYearProject_BE.Data;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject_BE.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateCategory(CategoryModel category)
        {
            category.IsDeleted = false;
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CategoryModel>> GetAllCategories()
        {
            return await _context.Categories
                .Where(c => c.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<CategoryModel> GetCategoryById(int id)
        {
            var category = await _context.Categories
                .Where(c => c.IsDeleted == false && c.Id == id)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }
            return category;
        }

        public async Task UpdateCategory(CategoryModel category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteCategory(int id)
        {
            var category = await GetCategoryById(id);
            category.IsDeleted = true;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task RestoreCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            category.IsDeleted = false;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task HardDeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByName(string name)
        {
            return await _context.Categories.AnyAsync(c => c.Name == name);
        }
    }
}
