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
        private readonly ICourseService _courseService;

        public CategoryController(ICategoryService categoryService, ICourseService courseService)
        {
            _categoryService = categoryService;
            _courseService = courseService;
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
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDTO categoryDto)
        {
            try
            {
                await _categoryService.UpdateCategory(id, categoryDto);
                return Ok("Category updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDeleteCategory(int id)
        {
            try
            {
                await _categoryService.SoftDeleteCategory(id);
                return Ok("Category deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT: api/Category/Restore/5
        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreCategory(int id)
        {
            try
            {
                await _categoryService.RestoreCategory(id);
                return Ok("Category restored successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("HardDelete/{id}")]
        public async Task<IActionResult> HardDeleteCategory(int id)
        {
            try
            {
                await _categoryService.HardDeleteCategory(id);
                return Ok("Category was permanently deleted.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/Courses")]
        public async Task<IActionResult> GetCoursesByCategoryId(int id)
        {
            try
            {
                var courses = await _courseService.GetCoursesByCategoryId(id);
                return Ok(courses);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
