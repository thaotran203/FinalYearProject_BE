using FinalYearProject_BE.DTOs;

namespace FinalYearProject_BE.Services.IService
{
    public interface IFileService
    {
        Task<FileResponseDTO> CreateFile(FileUploadDTO fileDto);
        Task<List<FileResponseDTO>> GetFilesByLessonId(int lessonId);
        Task<FileResponseDTO> GetFileById(int fileId);
        Task<FileResponseDTO> UpdateFile(int fileId, FileUploadDTO fileDto);
        Task DeleteFile(int fileId);
    }
}
