using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;

namespace FinalYearProject_BE.Services.IService
{
    public interface IMessageService
    {
        Task SendMessageFromStudent(int studentId, int courseId, string messageContent);
        Task SendMessageFromInstructor(int instructorId, SendMessageDTO messageDto);
        Task<List<MessageDTO>> GetAllMessagesForTeacher(int teacherId, int courseId);
        Task<List<MessageDTO>> GetMessagesWithStudent(int teacherId, int studentId, int courseId);
        Task<List<MessageDTO>> GetMessagesWithTeacher(int studentId, int courseId);
    }
}
