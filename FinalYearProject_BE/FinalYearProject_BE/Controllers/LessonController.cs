﻿using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Services;
using FinalYearProject_BE.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace FinalYearProject_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLesson(LessonDTO lessonDto)
        {
            await _lessonService.CreateLesson(lessonDto);
            return CreatedAtAction(nameof(GetLessonById), new { id = lessonDto.Id }, lessonDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLessons()
        {
            var lessons = await _lessonService.GetAllLessons();
            return Ok(lessons);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLessonById(int id)
        {
            var lesson = await _lessonService.GetLessonById(id);
            if (lesson == null)
            {
                return NotFound();
            }
            return Ok(lesson);
        }

        [HttpGet("Course/{courseId}")]
        public async Task<IActionResult> GetLessonsByCourseId(int courseId)
        {
            var lessons = await _lessonService.GetLessonsByCourseId(courseId);
            return Ok(lessons);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLesson(int id, LessonDTO lessonDto)
        {
            try
            {
                await _lessonService.UpdateLesson(id, lessonDto);
                return Ok("Lesson updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDeleteLesson(int id)
        {
            try
            {
                await _lessonService.SoftDeleteLesson(id);
                return Ok("Lesson deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreLesson(int id)
        {
            try
            {
                await _lessonService.RestoreLesson(id);
                return Ok("Lesson restored successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("HardDelete/{id}")]
        public async Task<IActionResult> HardDeleteLesson(int id)
        {
            try
            {
                await _lessonService.HardDeleteLesson(id);
                return Ok("Lesson was permanently deleted.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("LessonProgress/{courseId}")]
        public async Task<IActionResult> GetLessonsWithProgressForSudent(int courseId)
        {
            var userId = int.Parse(User.FindFirst("Id")?.Value);
            if (userId == null)
                return Unauthorized("User ID not found in token.");

            var lessons = await _lessonService.GetLessonsWithProgressByCourseId(courseId, userId);
            return Ok(lessons);
        }

        [HttpPost("SaveProgress/{lessonId}")]
        public async Task<IActionResult> SaveLessonProgress(int lessonId)
        {
            if (lessonId == 0)
            {
                return BadRequest("Invalid lessonId.");
            }

            var userId = int.Parse(User.FindFirst("Id")?.Value);
            if (userId == null)
                return Unauthorized("User ID not found in token.");

            await _lessonService.SaveLessonProgress(userId, lessonId);
            return Ok();
        }
    }
}
