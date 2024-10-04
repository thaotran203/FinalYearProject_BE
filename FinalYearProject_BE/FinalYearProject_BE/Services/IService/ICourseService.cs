using FinalYearProject_BE.DTOs;

namespace FinalYearProject_BE.Services.IService
{
    public interface ICourseService
    {
        Task CreateCourse(CourseDTO courseDto);
        Task<List<CourseDTO>> GetAllCourses();
        Task<CourseDTO> GetCourseById(int id);
        Task UpdateCourse(int id, CourseDTO courseDto);
        Task SoftDeleteCourse(int id);
        Task RestoreCourse(int id);
        Task HardDeleteCourse(int id);
        Task<List<CourseDTO>> GetCoursesByCategoryId(int categoryId);
    }
}
