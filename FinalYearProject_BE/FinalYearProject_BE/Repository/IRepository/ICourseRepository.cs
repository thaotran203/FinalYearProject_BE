﻿using FinalYearProject_BE.Models;

namespace FinalYearProject_BE.Repository.IRepository
{
    public interface ICourseRepository
    {
        Task CreateCourse(CourseModel course);
        Task<List<CourseModel>> GetAllCourses();
        Task<CourseModel> GetCourseById(int id);
        Task UpdateCourse(CourseModel course);
        Task SoftDeleteCourse(int id);
        Task RestoreCourse(int id);
        Task HardDeleteCourse(int id);
        Task<List<CourseModel>> GetCoursesByCategoryId(int categoryId);
        Task<bool> ExistsByTitle(string title);
    }
}
