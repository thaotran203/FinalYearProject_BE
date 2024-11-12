using FinalYearProject_BE.Data;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;

namespace FinalYearProject_BE.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _context;

        public FileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateFile(FileModel file)
        {
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
        }

        public async Task<FileModel> GetFileById(int id) => await _context.Files.FindAsync(id);

        public async Task<List<FileModel>> GetFilesByLessonId(int lessonId) =>
            await _context.Files.Where(f => f.LessonId == lessonId).ToListAsync();

        public async Task UpdateFile(FileModel file)
        {
            _context.Files.Update(file);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFile(int id)
        {
            var file = await _context.Files.FindAsync(id);
            if (file != null)
            {
                _context.Files.Remove(file);
                await _context.SaveChangesAsync();
            }
        }
    }
}
