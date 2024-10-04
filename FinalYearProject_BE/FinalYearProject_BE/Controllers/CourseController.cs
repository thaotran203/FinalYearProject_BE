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
        public async Task<IActionResult> CreateCategory(CourseDTO courseDto)
        {
            if (string.IsNullOrWhiteSpace(courseDto.Title))
            {
                return BadRequest("Course title must not be empty.");
            }

            await _courseService.CreateCourse(courseDto);
            return CreatedAtAction("GetCourseById", new { id = courseDto.Id }, courseDto);
        }
    }
}
