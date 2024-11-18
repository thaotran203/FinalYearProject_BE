using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalYearProject_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonVideoController : ControllerBase
    {
        private readonly ILessonVideoService _lessonVideoService;

        public LessonVideoController(ILessonVideoService lessonVideoService)
        {
            _lessonVideoService = lessonVideoService;
        }

        [HttpPost]
        public async Task<IActionResult> AddVideo([FromForm] VideoUploadDTO videoUploadDto)
        {
            var file = await _lessonVideoService.AddVideo(videoUploadDto);
            return Ok(file);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVidesById(int id)
        {
            var result = await _lessonVideoService.GetVideoById(id);
            return Ok(result);
        }

        [HttpGet("Lesson/{lessonId}")]
        public async Task<IActionResult> GetVideosByLessonId(int lessonId)
        {
            var files = await _lessonVideoService.GetVideosByLessonId(lessonId);
            return Ok(files);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVideo(int videoId, [FromForm] VideoUploadDTO videoUploadDto)
        {
            var file = await _lessonVideoService.UpdateVideo(videoId, videoUploadDto);
            return Ok("Video updated successfully.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteVideo(int videoId)
        {
            await _lessonVideoService.DeleteVideo(videoId);
            return Ok("Video deleted successfully.");
        }
    }
}
