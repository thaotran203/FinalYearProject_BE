using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;

namespace FinalYearProject_BE.Repository.IRepository
{
    public interface ILessonRepository
    {
        Task<List<LessonModel>> GetAllLessons();
        Task<LessonModel> GetLessonById(int id);
        Task<List<LessonModel>> GetLessonsByCourseId(int courseId);
        Task CreateLesson(LessonModel lesson);
        Task UpdateLesson(LessonModel lesson);
        Task SoftDeleteLesson(int id);
        Task RestoreLesson(int id);
        Task HardDeleteLesson(int id);

        Task<IEnumerable<LessonProgressDTO>> GetLessonsWithProgressByCourseId(int userId, int courseId);
        Task SaveLessonProgress(int userId, int lessonId);
    }
}
