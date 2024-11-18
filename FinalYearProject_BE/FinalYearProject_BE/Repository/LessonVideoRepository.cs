using FinalYearProject_BE.Data;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;

namespace FinalYearProject_BE.Repository
{
    public class LessonVideoRepository : ILessonVideoRepository
    {
        private readonly ApplicationDbContext _context;

        public LessonVideoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddVideo(LessonVideoModel video)
        {
            _context.LessonVideos.Add(video);
            await _context.SaveChangesAsync();
        }

        public async Task<LessonVideoModel> GetVideoById(int id) => await _context.LessonVideos.FindAsync(id);

        public async Task<List<LessonVideoModel>> GetVideosByLessonId(int lessonId) =>
            await _context.LessonVideos.Where(f => f.LessonId == lessonId).ToListAsync();

        public async Task UpdateVideo(LessonVideoModel video)
        {
            _context.LessonVideos.Update(video);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVideo(int id)
        {
            var video = await _context.LessonVideos.FindAsync(id);
            if (video != null)
            {
                _context.LessonVideos.Remove(video);
                await _context.SaveChangesAsync();
            }
        }
    }
}
