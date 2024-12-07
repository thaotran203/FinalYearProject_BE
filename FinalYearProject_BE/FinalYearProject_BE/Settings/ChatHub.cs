using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Services.IService;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace FinalYearProject_BE.Settings
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly ICourseService _courseService;

        public ChatHub(IMessageService messageService, ICourseService courseService)
        {
            _messageService = messageService;
            _courseService = courseService;
        }

        public async Task SendMessageFromStudent(int courseId, string messageContent)
        {
            var studentId = int.Parse(Context.UserIdentifier);
            var messageDto = new SendMessageDTO
            {
                CourseId = courseId,
                MessageContent = messageContent
            };

            try
            {
                await _messageService.SendMessageFromStudent(studentId, courseId, messageContent);

                var course = await _courseService.GetCourseById(courseId);
                var instructorId = course.InstructorId;

                await Clients.User(instructorId.ToString()).SendAsync("ReceiveMessage", new
                {
                    SenderId = studentId,
                    CourseId = courseId,
                    Content = messageContent,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveMessageError", ex.Message);
            }
        }

        public async Task SendMessageFromInstructor(int studentId, int courseId, string messageContent)
        {
            var instructorId = int.Parse(Context.UserIdentifier); 
            var messageDto = new SendMessageDTO
            {
                CourseId = courseId,
                StudentId = studentId,
                MessageContent = messageContent
            };

            try
            {
                await _messageService.SendMessageFromInstructor(instructorId, messageDto);

                // Thông báo qua SignalR cho học sinh
                await Clients.User(studentId.ToString()).SendAsync("ReceiveMessage", new
                {
                    SenderId = instructorId,
                    CourseId = courseId,
                    Content = messageContent,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveMessageError", ex.Message);
            }
        }

        public async Task ReceiveMessage(int receiverId, string messageContent)
        {
            await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", new
            {
                Content = messageContent,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
