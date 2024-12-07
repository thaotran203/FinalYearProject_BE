using FinalYearProject_BE.Data;
using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using FinalYearProject_BE.Services.IService;
using FinalYearProject_BE.Settings;
using Google;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject_BE.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourseRepository _courseRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IHubContext<ChatHub> _chatHubContext;


        public MessageService(ApplicationDbContext context, IMessageRepository messageRepository, ICourseRepository courseRepository, IHubContext<ChatHub> chatHubContext)
        {
            _context = context;
            _messageRepository = messageRepository;
            _courseRepository = courseRepository;
            _chatHubContext = chatHubContext;
        }

        public async Task SendMessageFromStudent(int studentId, int courseId, string messageContent)
        {
            var course = await _courseRepository.GetCourseById(courseId);
            if (course == null)
            {
                throw new Exception("Course not found.");
            }

            var isEnrolled = await _messageRepository.IsUserEnrolledInCourse(courseId, studentId);
            if (!isEnrolled)
            {
                throw new UnauthorizedAccessException("Student is not enrolled in this course.");
            }

            var receiverId = course.InstructorId;

            // Kiểm tra nếu học sinh gửi tin nhắn cho chính mình
            if (studentId == receiverId)
            {
                throw new Exception("You cannot send messages to yourself.");
            }

            var message = new MessageModel
            {
                SenderId = studentId,
                ReceiverId = receiverId,
                CourseId = courseId,
                MessageContent = messageContent,
                TimeStamp = DateTime.UtcNow
            };

            // Lưu tin nhắn vào database
            await _messageRepository.SaveMessage(message);

            // Thông báo cho giáo viên qua SignalR
            await _chatHubContext.Clients.User(receiverId.ToString())
                .SendAsync("ReceiveMessage", new
                {
                    SenderId = studentId,
                    CourseId = courseId,
                    Content = messageContent,
                    Timestamp = message.TimeStamp
                });
        }


        public async Task SendMessageFromInstructor(int instructorId, SendMessageDTO messageDto)
        {
            var course = await _courseRepository.GetCourseById(messageDto.CourseId);
            if (course == null)
            {
                throw new Exception("Course not found.");
            }

            // Kiểm tra nếu giáo viên là giảng viên của khóa học
            if (course.InstructorId != instructorId)
            {
                throw new UnauthorizedAccessException("You are not authorized to send messages for this course.");
            }

            var receiverId = messageDto.StudentId ?? throw new Exception("StudentId must be provided.");

            // Kiểm tra nếu học sinh đã đăng ký khóa học
            var isEnrolled = await _messageRepository.IsUserEnrolledInCourse(messageDto.CourseId, receiverId);
            if (!isEnrolled)
            {
                throw new UnauthorizedAccessException("Student is not enrolled in this course.");
            }

            var message = new MessageModel
            {
                SenderId = instructorId,
                ReceiverId = receiverId,
                CourseId = messageDto.CourseId,
                MessageContent = messageDto.MessageContent,
                TimeStamp = DateTime.UtcNow
            };

            // Lưu tin nhắn vào database
            await _messageRepository.SaveMessage(message);

            // Thông báo cho học sinh qua SignalR
            await _chatHubContext.Clients.User(receiverId.ToString())
                .SendAsync("ReceiveMessage", new
                {
                    SenderId = instructorId,
                    CourseId = messageDto.CourseId,
                    Content = messageDto.MessageContent,
                    Timestamp = message.TimeStamp
                });
        }


        public async Task<List<MessageDTO>> GetAllMessagesForTeacher(int teacherId, int courseId)
        {
            var course = await _courseRepository.GetCourseById(courseId);
            if (course == null || course.InstructorId != teacherId)
            {
                throw new UnauthorizedAccessException("Teacher is not authorized for this course.");
            }

            var messages = await _messageRepository.GetMessagesByCourseAndInstructor(courseId, teacherId);

            if (messages == null || !messages.Any())
            {
                return new List<MessageDTO>();
            }

            var users = await _context.Users
                .Where(u => messages.Select(m => m.SenderId).Contains(u.Id))
                .ToListAsync();

            var messageDTOs = messages.Select(m =>
            {
                var sender = users.FirstOrDefault(u => u.Id == m.SenderId);

                return new MessageDTO
                {
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.MessageContent,
                    CourseId = m.CourseId,
                    Timestamp = m.TimeStamp,
                    SenderName = sender?.FullName // Fetch the student's name using SenderId
                };
            }).ToList();

            return messageDTOs;
        }



        public async Task<List<MessageDTO>> GetMessagesWithStudent(int teacherId, int studentId, int courseId)
        {
            var course = await _courseRepository.GetCourseById(courseId);
            if (course == null || course.InstructorId != teacherId)
            {
                throw new UnauthorizedAccessException("Teacher is not authorized for this course.");
            }

            var userCourse = await _messageRepository.IsUserEnrolledInCourse(courseId, studentId);
            if (userCourse == false)
            {
                throw new UnauthorizedAccessException("Student is not enrolled in this course.");
            }

            var messages = await _messageRepository.GetMessagesBetweenUserId(teacherId, studentId, courseId);

            if (messages == null || !messages.Any())
            {
                return new List<MessageDTO>();
            }

            return messages.Select(m => new MessageDTO
            {
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.MessageContent,
                CourseId = m.CourseId,
                Timestamp = m.TimeStamp
            }).ToList();
        }

        public async Task<List<MessageDTO>> GetMessagesWithTeacher(int studentId, int courseId)
        {
            var userCourse = await _messageRepository.IsUserEnrolledInCourse(courseId, studentId);
            if (userCourse == false)
            {
                throw new UnauthorizedAccessException("Student is not enrolled in this course.");
            }

            var course = await _courseRepository.GetCourseById(courseId);
            if (course == null)
            {
                throw new InvalidOperationException("Course not found.");
            }

            var teacherId = course.InstructorId;

            var messages = await _messageRepository.GetMessagesBetweenUserId(studentId, teacherId, courseId);

            if (messages == null || !messages.Any())
            {
                return new List<MessageDTO>();
            }

            return messages.Select(m => new MessageDTO
            {
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.MessageContent,
                CourseId = m.CourseId,
                Timestamp = m.TimeStamp
            }).ToList();
        }
    }
}
