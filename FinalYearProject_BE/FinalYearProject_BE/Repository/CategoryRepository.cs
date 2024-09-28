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
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CategoryModel>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<CategoryModel> GetCategoryById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task UpdateCategory(CategoryModel category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(CategoryModel category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByName(string name)
        {
            return await _context.Categories.AnyAsync(c => c.Name == name);
        }
    }
}
