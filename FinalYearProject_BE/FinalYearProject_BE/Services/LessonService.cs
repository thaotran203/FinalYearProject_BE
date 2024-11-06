using AutoMapper;
using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository;
using FinalYearProject_BE.Repository.IRepository;
using FinalYearProject_BE.Services.IService;

namespace FinalYearProject_BE.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;

        public LessonService(ILessonRepository lessonRepository, IMapper mapper)
        {
            _lessonRepository = lessonRepository;
            _mapper = mapper;
        }

        public async Task CreateLesson(LessonDTO lessonDto)
        {
            var lesson = _mapper.Map<LessonModel>(lessonDto);
            await _lessonRepository.CreateLesson(lesson);
        }

        public async Task<List<LessonDTO>> GetAllLessons()
        {
            var lessons = await _lessonRepository.GetAllLessons();
            return _mapper.Map<List<LessonDTO>>(lessons);
        }

        public async Task<LessonDTO> GetLessonById(int id)
        {
            var lesson = await _lessonRepository.GetLessonById(id);
            return _mapper.Map<LessonDTO>(lesson);
        }

        public async Task<List<LessonDTO>> GetLessonsByCourseId(int courseId)
        {
            var lessons = await _lessonRepository.GetLessonsByCourseId(courseId);
            return _mapper.Map<List<LessonDTO>>(lessons);
        }

        public async Task UpdateLesson(int id, LessonDTO lessonDto)
        {
            var lesson = await _lessonRepository.GetLessonById(id);

            lesson.Title = lessonDto.Title;
            lesson.Description = lessonDto.Description;
            lesson.CourseId = lessonDto.CourseId;

            await _lessonRepository.UpdateLesson(lesson);
        }
    }
}
