using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;

namespace FinalYearProject_BE.Repository.IRepository
{
    public interface ICourseRepository
    {
        Task CreateCourse(CourseModel course);
        Task<List<CourseResponseDTO>> GetAllCourses();
        Task<CourseResponseDTO> GetCourseById(int id);
        Task<CourseModel> GetCourseEntityById(int id);
        Task<List<CourseResponseDTO>> GetAllCourseForAdmin();
        Task<List<CourseResponseDTO>> GetCoursesByInstructorId(int teacherId);
        Task UpdateCourse(CourseModel course);
        Task SoftDeleteCourse(int id);
        Task RestoreCourse(int id);
        Task HardDeleteCourse(int id);
        Task<List<CourseModel>> GetCoursesByCategoryId(int categoryId);
        Task<bool> ExistsByTitle(string title);
    }
}
