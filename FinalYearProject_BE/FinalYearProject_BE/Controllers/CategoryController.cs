using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalYearProject_BE.Services;
using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Services.IService;
using FinalYearProject_BE.Models;

namespace FinalYearProject_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // POST: api/Category
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDTO categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest("Category data is null.");
            }

            if (string.IsNullOrWhiteSpace(categoryDto.Name))
            {
                return BadRequest("Category name must not be empty.");
            }

            await _categoryService.CreateCategory(categoryDto);
            return CreatedAtAction("GetCategoryById", new { id = categoryDto.Id }, categoryDto);
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategories();
            return Ok(categories);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDTO categoryDto)
        {
            if (string.IsNullOrWhiteSpace(categoryDto.Name))
            {
                return BadRequest("Category name must not be empty.");
            }
            await _categoryService.UpdateCategory(id, categoryDto);
            return Ok("Category updated successfully.");
        }
    }
}
