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

            if (!IsValidImageFileType(courseDto.Image))
            {
                throw new InvalidOperationException("Invalid image file type. Please choose again.");
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
            return courses;
        }

        public async Task<CourseResponseDTO> GetCourseById(int id)
        {
            var course = await _courseRepository.GetCourseById(id);
            return course;
        }

        public async Task UpdateCourse(int id, CourseDTO courseDto)
        {
            if (await _courseRepository.ExistsByTitle(courseDto.Title))
            {
                throw new InvalidOperationException("A course with the same title already exists.");
            }

            var course = await _courseRepository.GetCourseEntityById(id);

            if (course == null)
            {
                throw new Exception("Course not found.");
            }

            await _cloudinaryService.DeleteFile(course.ImageLink);

            if (!IsValidImageFileType(courseDto.Image))
            {
                throw new InvalidOperationException("Invalid image file type. Please choose again.");
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

        public async Task<List<CourseResponseDTO>> GetAllCourseForAdmin()
        {
            return await _courseRepository.GetAllCourseForAdmin();
        }

        public async Task<List<CourseResponseDTO>> GetCoursesByInstructorId(int teacherId)
        {
            return await _courseRepository.GetCoursesByInstructorId(teacherId);
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
            var course = await _courseRepository.GetCourseById(id);

            if (course == null)
            {
                throw new Exception("Course not found.");
            }

            await _cloudinaryService.DeleteFile(course.ImageLink);
            await _courseRepository.HardDeleteCourse(id);
        }

        public async Task<List<CourseResponseDTO>> GetCoursesByCategoryId(int categoryId)
        {
            var courses = await _courseRepository.GetCoursesByCategoryId(categoryId);
            return _mapper.Map<List<CourseResponseDTO>>(courses);
        }

        private bool IsValidImageFileType(IFormFile image)
        {
            var extension = Path.GetExtension(image.FileName)?.ToLower();

            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

            return allowedImageExtensions.Contains(extension);
        }
    }
}
