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

        public async Task<IEnumerable<LessonProgressDTO>> GetLessonsWithProgressByCourseId(int userId, int courseId)
        {
            var lessons = await _context.Lessons
                .Where(l => l.CourseId == courseId && !l.IsDeleted)
                .OrderBy(l => l.Id)
                .ToListAsync();

            var progress = await _context.LessonProgresses
                .Where(p => p.UserId == userId && lessons.Select(l => l.Id).Contains(p.LessonId))
                .ToListAsync();

            var lessonWithProgress = lessons.Select(l => new LessonProgressDTO
            {
                Id = l.Id,
                Title = l.Title,
                Description = l.Description,
                IsCompleted = progress.Any(p => p.LessonId == l.Id && p.IsCompleted)
            });

            return lessonWithProgress;
        }

        public async Task SaveLessonProgress(int userId, int lessonId)
        {
            var lessonProgress = new LessonProgressModel
            {
                Id = userId,
                LessonId = lessonId,
                IsCompleted = true
            };
            await _context.LessonProgresses.AddAsync(lessonProgress);
            await _context.SaveChangesAsync();
        }
    }
}
