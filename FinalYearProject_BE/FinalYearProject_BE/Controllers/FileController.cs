using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalYearProject_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFile([FromForm] FileUploadDTO fileDto)
        {
            var file = await _fileService.CreateFile(fileDto);
            return Ok(file);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFileById(int id)
        {
            var result = await _fileService.GetFileById(id);
            return Ok(result);
        }

        [HttpGet("Lesson/{lessonId}")]
        public async Task<IActionResult> GetFilesByLessonId(int lessonId)
        {
            var files = await _fileService.GetFilesByLessonId(lessonId);
            return Ok(files);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFile(int fileId, [FromForm] FileUploadDTO fileDto)
        {
            var file = await _fileService.UpdateFile(fileId, fileDto);
            return Ok("File updated successfully.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFile(int filed)
        {
            await _fileService.DeleteFile(filed);
            return Ok("File deleted successfully.");
        }
    }
}
