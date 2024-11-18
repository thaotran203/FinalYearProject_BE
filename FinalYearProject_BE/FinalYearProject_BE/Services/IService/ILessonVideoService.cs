using FinalYearProject_BE.DTOs;

namespace FinalYearProject_BE.Services.IService
{
    public interface ILessonVideoService
    {
        Task<VideoResponseDTO> AddVideo(VideoUploadDTO videoUploadDto);
        Task<List<VideoResponseDTO>> GetVideosByLessonId(int lessonId);
        Task<VideoResponseDTO> GetVideoById(int videoId);
        Task<VideoResponseDTO> UpdateVideo(int videoId, VideoUploadDTO videoUploadDto);
        Task DeleteVideo(int videoId);
    }
}
