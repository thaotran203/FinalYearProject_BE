using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using FinalYearProject_BE.Services.IService;
using Humanizer;
using NuGet.Protocol.Core.Types;

namespace FinalYearProject_BE.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public CourseService (ICourseRepository courseRepository, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task CreateCourse(CourseDTO courseDto)
        {
            if (await _courseRepository.ExistsByTitle(courseDto.Title)) 
            {
                throw new InvalidOperationException("A course with the same title already exists.");
            }

            if (courseDto.Image == null || courseDto.Image.Length == 0)
            {
                throw new Exception("No file uploaded.");
            }
            var courseImage = await _cloudinaryService.UploadCourseImage(courseDto.Image);

            var course = new CourseModel
            {
                Title = courseDto.Title,
                Description = courseDto.Description,
                CourseContent = courseDto.CourseContent,
                Price = courseDto.Price,
                ImageLink = courseImage,
                CategoryId = courseDto.CategoryId,
                InstructorId = courseDto.InstructorId
            };
            await _courseRepository.CreateCourse(course);
        }

        public async Task<List<CourseResponseDTO>> GetAllCourses()
        {
            var courses = await _courseRepository.GetAllCourses();
            return _mapper.Map<List<CourseResponseDTO>>(courses);
        }

        public async Task<CourseResponseDTO> GetCourseById(int id)
        {
            var course = await _courseRepository.GetCourseById(id);
            return _mapper.Map<CourseResponseDTO>(course);
        }

        public async Task UpdateCourse(int id, CourseDTO courseDto)
        {
            var course = await _courseRepository.GetCourseById(id);

            if (course == null)
            {
                throw new Exception("Course not found.");
            }

            if (courseDto.Image == null || courseDto.Image.Length == 0)
            {
                throw new Exception("No file uploaded.");
            }
            var courseImage = await _cloudinaryService.UploadCourseImage(courseDto.Image);

            course.Title = courseDto.Title;
            course.Description = courseDto.Description;
            course.CourseContent = courseDto.CourseContent;
            course.Price = courseDto.Price;
            course.ImageLink = courseImage;
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

        public async Task<List<CourseResponseDTO>> GetCoursesByCategoryId(int categoryId)
        {
            var courses = await _courseRepository.GetCoursesByCategoryId(categoryId);
            return _mapper.Map<List<CourseResponseDTO>>(courses);
        }
    }
}
