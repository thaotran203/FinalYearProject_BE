using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;

namespace FinalYearProject_BE.Repository.IRepository
{
    public interface IMessageRepository
    {
        Task SaveMessage(MessageModel message);
        Task<List<MessageModel>> GetMessagesByCourseAndInstructor(int courseId, int instructorId);
        Task<List<MessageModel>> GetMessagesBetweenUserId(int userId1, int userId2, int courseId);
        Task<bool> IsUserEnrolledInCourse(int courseId, int userId);
    }
}
