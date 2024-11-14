using FinalYearProject_BE.DTOs;

namespace FinalYearProject_BE.Services.IService
{
    public interface ILessonService
    {
        Task<List<LessonDTO>> GetAllLessons();
        Task<LessonDTO> GetLessonById(int id);
        Task<List<LessonDTO>> GetLessonsByCourseId(int courseId);
        Task CreateLesson(LessonDTO lessonDto);
        Task UpdateLesson(int id, LessonDTO lessonDto);
        Task SoftDeleteLesson(int id);
        Task RestoreLesson(int id);
        Task HardDeleteLesson(int id);

        Task<IEnumerable<LessonProgressDTO>> GetLessonsWithProgressByCourseId(int userId, int courseId);
        Task SaveLessonProgress(int userId, int lessonId);
    }
}
