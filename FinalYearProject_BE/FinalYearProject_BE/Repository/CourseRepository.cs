using FinalYearProject_BE.Data;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace FinalYearProject_BE.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateCourse(CourseModel course)
        {
            course.IsDeleted = false;
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CourseModel>> GetAllCourses()
        {
            return await _context.Courses
                .Where(c => c.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<CourseModel> GetCourseById(int id)
        {
            var course = await _context.Courses
                .Where(c => c.IsDeleted == false && c.Id == id)
                .FirstOrDefaultAsync();

            if (course == null) throw new KeyNotFoundException("Course not found.");
            return course;
        }

        public async Task UpdateCourse(CourseModel course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteCourse(int id)
        {
            var course = await GetCourseById(id);
            course.IsDeleted = true;
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task RestoreCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) throw new KeyNotFoundException("Course not found.");

            course.IsDeleted = false; 
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task HardDeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) throw new KeyNotFoundException("Course not found.");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CourseModel>> GetCoursesByCategoryId(int categoryId)
        {
            return await _context.Courses
                .Where(c => c.CategoryId == categoryId && c.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<bool> ExistsByTitle(string title)
        {
            return await _context.Courses.AnyAsync(c => c.Title == title);
        }
    }
}
