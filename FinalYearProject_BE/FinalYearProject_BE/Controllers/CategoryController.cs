using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalYearProject_BE.Data;
using FinalYearProject_BE.Models;

namespace FinalYearProject_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<CategoryModel>> CreateCategory(CategoryModel categoryModel)
        {
            if (categoryModel == null)
            {
                return BadRequest("Category data is null.");
            }

            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories' is null.");
            }
            _context.Categories.Add(categoryModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoryById", new { id = categoryModel.Id }, categoryModel);
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetCategories()
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            return await _context.Categories.ToListAsync();
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryModel>> GetCategoryById(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }
    }
}
