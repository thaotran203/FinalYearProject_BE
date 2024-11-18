using AutoMapper;
using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using FinalYearProject_BE.Services.IService;

namespace FinalYearProject_BE.Services
{
    public class LessonVideoService : ILessonVideoService
    {
        private readonly ILessonVideoRepository _lessonVideoRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;

        public LessonVideoService(ILessonVideoRepository lessonVideoRepository, ICloudinaryService cloudinaryService, IMapper mapper)
        {
            _lessonVideoRepository = lessonVideoRepository;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
        }

        public async Task<VideoResponseDTO> AddVideo(VideoUploadDTO videoUploadDto)
        {
            if (!IsValidVideoFileType(videoUploadDto.Video))
            {
                throw new InvalidOperationException("Invalid file type. Only .mp4 files are allowed.");
            }

            var videoUrl = await _cloudinaryService.UploadLessonVideo(videoUploadDto.Video);

            var lessonVideoModel = new LessonVideoModel
            {
                VideoUrl = videoUrl,
                LessonId = videoUploadDto.LessonId
            };

            await _lessonVideoRepository.AddVideo(lessonVideoModel);
            return _mapper.Map<VideoResponseDTO>(lessonVideoModel);
        }

        public async Task<List<VideoResponseDTO>> GetVideosByLessonId(int lessonId)
        {
            var videos = await _lessonVideoRepository.GetVideosByLessonId(lessonId);
            return _mapper.Map<List<VideoResponseDTO>>(videos);
        }

        public async Task<VideoResponseDTO> GetVideoById(int videoId)
        {
            var video = await _lessonVideoRepository.GetVideoById(videoId);
            return _mapper.Map<VideoResponseDTO>(video);
        }

        public async Task<VideoResponseDTO> UpdateVideo(int videoId, VideoUploadDTO videoUploadDto)
        {
            var video = await _lessonVideoRepository.GetVideoById(videoId);
            if (video == null) throw new Exception("Video not found.");

            await _cloudinaryService.DeleteFile(video.VideoUrl);

            if (!IsValidVideoFileType(videoUploadDto.Video))
            {
                throw new InvalidOperationException("Invalid file type. Only .mp4 files are allowed.");
            }

            var newFileUrl = await _cloudinaryService.UploadLessonVideo(videoUploadDto.Video);
            video.VideoUrl = newFileUrl;

            await _lessonVideoRepository.UpdateVideo(video);

            return _mapper.Map<VideoResponseDTO>(video);
        }

        public async Task DeleteVideo(int videoId)
        {
            var video = await _lessonVideoRepository.GetVideoById(videoId);
            if (video == null) throw new Exception("Video not found.");

            await _cloudinaryService.DeleteFile(video.VideoUrl);
            await _lessonVideoRepository.DeleteVideo(videoId);
        }

        private bool IsValidVideoFileType(IFormFile video)
        {
            var extension = Path.GetExtension(video.FileName)?.ToLower();

            return extension == ".mp4";
        }
    }
}
