using FinalYearProject_BE.DTOs;
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
    }
}
