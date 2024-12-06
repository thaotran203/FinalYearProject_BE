using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;

namespace FinalYearProject_BE.Services.IService
{
    public interface ICourseService
    {
        Task CreateCourse(CourseDTO courseDto);
        Task<List<CourseResponseDTO>> GetAllCourses();
        Task<CourseResponseDTO> GetCourseById(int id);
        Task<List<CourseResponseDTO>> GetAllCourseForAdmin();
        Task<List<CourseResponseDTO>> GetCoursesByInstructorId(int teacherId);
        Task<List<(string FullName, string Email, string PhoneNumber, double? Grade, DateTime? TestDate)>> GetStudentsInCourse(int courseId, string? searchQuery = null);
        Task UpdateCourse(int id, CourseDTO courseDto);
        Task SoftDeleteCourse(int id);
        Task RestoreCourse(int id);
        Task HardDeleteCourse(int id);
        Task<List<CourseResponseDTO>> GetCoursesByCategoryId(int categoryId);
    }
}
