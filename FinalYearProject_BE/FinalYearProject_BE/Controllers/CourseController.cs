using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalYearProject_BE.Data;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Services.IService;
using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Services;

namespace FinalYearProject_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // POST: api/Category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CourseDTO courseDto)
        {
            if (string.IsNullOrWhiteSpace(courseDto.Title))
            {
                return BadRequest("Course title must not be empty.");
            }

            await _courseService.CreateCourse(courseDto);
            return CreatedAtAction("GetCourseById", new { id = courseDto.Id }, courseDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCourses();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromForm] CourseDTO courseDto)
        {
            try
            {
                await _courseService.UpdateCourse(id, courseDto);
                return Ok("Course updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDeleteCourse(int id)
        {
            try
            {
                await _courseService.SoftDeleteCourse(id);
                return Ok("Course deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreCourse(int id)
        {
            try
            {
                await _courseService.RestoreCourse(id);
                return Ok("Course restored successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("HardDelete/{id}")]
        public async Task<IActionResult> HardDeleteCourse(int id)
        {
            try
            {
                await _courseService.HardDeleteCourse(id);
                return Ok("Category was permanently deleted.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
