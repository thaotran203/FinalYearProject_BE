using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalYearProject_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public ChatController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("send-from-student")]
        public async Task<IActionResult> SendMessageFromStudent(int courseId, string messageContent)
        {
            var studentId = int.Parse(User.FindFirst("Id")?.Value);
            try
            {
                await _messageService.SendMessageFromStudent(studentId, courseId, messageContent);
                return Ok(new { Message = "Message sent successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("send-from-instructor")]
        public async Task<IActionResult> SendMessageFromInstructor([FromBody] SendMessageDTO messageDto)
        {
            var instructorId = int.Parse(User.FindFirst("Id")?.Value);
            try
            {
                await _messageService.SendMessageFromInstructor(instructorId, messageDto);
                return Ok(new { Message = "Message sent successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{courseId}/messages")]
        public async Task<IActionResult> GetMessagesForTeacher(int courseId)
        {
            var teacherId = int.Parse(User.FindFirst("Id")?.Value);
            var messages = await _messageService.GetAllMessagesForTeacher(teacherId, courseId);

            return Ok(messages);
        }

        [HttpGet("teacher/{courseId}/messages/{studentId}")]
        public async Task<IActionResult> GetMessagesWithStudent(int courseId, int studentId)
        {
            var teacherId = int.Parse(User.FindFirst("Id")?.Value);
            var messages = await _messageService.GetMessagesWithStudent(teacherId, studentId, courseId);

            return Ok(messages);
        }

        [HttpGet("student/{courseId}/messages")]
        public async Task<IActionResult> GetMessagesWithTeacher(int courseId)
        {
            var studentId = int.Parse(User.FindFirst("Id")?.Value);
            var messages = await _messageService.GetMessagesWithTeacher(studentId, courseId);

            return Ok(messages);
        }
    }
}
