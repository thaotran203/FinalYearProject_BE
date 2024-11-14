using FinalYearProject_BE.DTOs;

namespace FinalYearProject_BE.Services.IService
{
    public interface ICourseService
    {
        Task CreateCourse(CourseDTO courseDto);
        Task<List<CourseResponseDTO>> GetAllCourses();
        Task<CourseResponseDTO> GetCourseById(int id);
        Task UpdateCourse(int id, CourseDTO courseDto);
        Task SoftDeleteCourse(int id);
        Task RestoreCourse(int id);
        Task HardDeleteCourse(int id);
        Task<List<CourseResponseDTO>> GetCoursesByCategoryId(int categoryId);
    }
}
