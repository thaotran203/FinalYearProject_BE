using AutoMapper;
using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using FinalYearProject_BE.Services.IService;

namespace FinalYearProject_BE.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CourseService (ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task CreateCourse(CourseDTO courseDto)
        {
            if (await _courseRepository.ExistsByTitle(courseDto.Title)) 
            {
                throw new InvalidOperationException("A course with the same title already exists.");
            }

            var course = _mapper.Map<CourseModel>(courseDto);
            await _courseRepository.CreateCourse(course);
        }

        public async Task<List<CourseDTO>> GetAllCourses()
        {
            var courses = await _courseRepository.GetAllCourses();
            return _mapper.Map<List<CourseDTO>>(courses);
        }

        public async Task<CourseDTO> GetCourseById(int id)
        {
            var course = await _courseRepository.GetCourseById(id);
            return _mapper.Map<CourseDTO>(course);
        }

        public async Task UpdateCourse(int id, CourseDTO courseDto)
        {
            var course = await _courseRepository.GetCourseById(id);

            course.Title = courseDto.Title;
            course.Description = courseDto.Description;
            course.Price = courseDto.Price;
            course.CategoryId = courseDto.CategoryId;
            course.InstructorId = courseDto.InstructorId;

            await _courseRepository.UpdateCourse(course);
        }

        public async Task SoftDeleteCourse(int id)
        {
            await _courseRepository.SoftDeleteCourse(id);
        }

        public async Task RestoreCourse(int id)
        {
            await _courseRepository.RestoreCourse(id);
        }
        
        public async Task HardDeleteCourse(int id)
        {
            await _courseRepository.HardDeleteCourse(id);
        }

        public async Task<List<CourseDTO>> GetCoursesByCategoryId(int categoryId)
        {
            var courses = await _courseRepository.GetCoursesByCategoryId(categoryId);
            return _mapper.Map<List<CourseDTO>>(courses);
        }
    }
}
