using FinalYearProject_BE.Data;
using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;

namespace FinalYearProject_BE.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveMessage(MessageModel message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MessageModel>> GetMessagesBetweenUserId(int userId1, int userId2, int courseId)
        {
            var messages = await _context.Messages
                .Where(m => m.CourseId == courseId &&
                            ((m.SenderId == userId1 && m.ReceiverId == userId2) ||
                             (m.SenderId == userId2 && m.ReceiverId == userId1)))
                .OrderBy(m => m.TimeStamp)
                .ToListAsync();

            return messages;
        }

        public async Task<List<MessageModel>> GetMessagesByCourseAndInstructor(int courseId, int instructorId)
        {
            var messages = await _context.Messages
                .Where(m => m.CourseId == courseId &&
                            (m.ReceiverId == instructorId))
                .OrderBy(m => m.TimeStamp)
                .ToListAsync();

            return messages;
        }

        public async Task<bool> IsUserEnrolledInCourse(int courseId, int userId)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId);

            return enrollment != null;
        }
    }
}
