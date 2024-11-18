using FinalYearProject_BE.Models;

namespace FinalYearProject_BE.Repository.IRepository
{
    public interface ILessonVideoRepository
    {
        Task AddVideo(LessonVideoModel video);
        Task<LessonVideoModel> GetVideoById(int id);
        Task<List<LessonVideoModel>> GetVideosByLessonId(int lessonId);
        Task UpdateVideo(LessonVideoModel video);
        Task DeleteVideo(int id);
    }
}
