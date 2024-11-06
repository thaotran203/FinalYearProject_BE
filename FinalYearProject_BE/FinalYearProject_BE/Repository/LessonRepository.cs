using FinalYearProject_BE.Data;
using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject_BE.Repository
{
    public class LessonRepository : ILessonRepository
    {
        private readonly ApplicationDbContext _context;

        public LessonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateLesson(LessonModel lesson)
        {
            lesson.IsDeleted = false;
            await _context.Lessons.AddAsync(lesson);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LessonModel>> GetAllLessons()
        {
            return await _context.Lessons
                .Where(l => l.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<LessonModel> GetLessonById(int id)
        {
            var lesson = await _context.Lessons
                .Where(l => l.IsDeleted == false && l.Id == id)
                .FirstOrDefaultAsync();

            if (lesson == null) throw new KeyNotFoundException("Lesson not found.");
            return lesson;
        }

        public async Task<List<LessonModel>> GetLessonsByCourseId(int courseId)
        {
            return await _context.Lessons.Where(l => l.CourseId == courseId && !l.IsDeleted).ToListAsync();
        }

        public async Task UpdateLesson(LessonModel lesson)
        {
            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteLesson(int id)
        {
            var lesson = await GetLessonById(id);
            lesson.IsDeleted = true;
            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();
        }

        public async Task RestoreLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                throw new KeyNotFoundException("Lesson not found.");
            }

            lesson.IsDeleted = false;
            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();
        }

        public async Task HardDeleteLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                throw new KeyNotFoundException("Lesson not found.");
            }

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
        }
    }
}
