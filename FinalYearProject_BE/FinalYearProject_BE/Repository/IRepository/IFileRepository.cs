using FinalYearProject_BE.Models;

namespace FinalYearProject_BE.Repository.IRepository
{
    public interface IFileRepository
    {
        Task CreateFile(FileModel file);
        Task<FileModel> GetFileById(int id);
        Task<List<FileModel>> GetFilesByLessonId(int lessonId);
        Task UpdateFile(FileModel file);
        Task DeleteFile(int id);
    }
}
