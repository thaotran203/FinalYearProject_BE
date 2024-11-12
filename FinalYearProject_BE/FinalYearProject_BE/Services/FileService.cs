using AutoMapper;
using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using FinalYearProject_BE.Services.IService;

namespace FinalYearProject_BE.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IGoogleDriveService _googleDriveService;
        private readonly IMapper _mapper;

        public FileService(IFileRepository fileRepository, IGoogleDriveService googleDriveService, IMapper mapper)
        {
            _fileRepository = fileRepository;
            _googleDriveService = googleDriveService;
            _mapper = mapper;
        }

        public async Task<FileResponseDTO> CreateFile(FileUploadDTO fileDto)
        {
            var fileUrl = await _googleDriveService.UploadFileToDrive(fileDto.File, fileDto.FileName);

            var fileModel = new FileModel
            {
                FileName = fileDto.FileName,
                FileUrl = fileUrl,
                FileType = fileDto.FileType,
                LessonId = fileDto.LessonId
            };

            await _fileRepository.CreateFile(fileModel);
            return _mapper.Map<FileResponseDTO>(fileModel);
        }

        public async Task<List<FileResponseDTO>> GetFilesByLessonId(int lessonId)
        {
            var files = await _fileRepository.GetFilesByLessonId(lessonId);
            return _mapper.Map<List<FileResponseDTO>>(files);
        }

        public async Task<FileResponseDTO> GetFileById(int fileId)
        {
            var file = await _fileRepository.GetFileById(fileId);
            return _mapper.Map<FileResponseDTO>(file);
        }

        public async Task<FileResponseDTO> UpdateFile(int fileId, FileUploadDTO fileDto)
        {
            var file = await _fileRepository.GetFileById(fileId);
            if (file == null) throw new Exception("File not found.");

            await _googleDriveService.DeleteFileFromDrive(file.FileUrl);

            var newFileUrl = await _googleDriveService.UploadFileToDrive(fileDto.File, fileDto.FileName);
            file.FileName = fileDto.FileName;
            file.FileUrl = newFileUrl;
            file.FileType = fileDto.FileType;

            await _fileRepository.UpdateFile(file);

            return _mapper.Map<FileResponseDTO>(file);
        }

        public async Task DeleteFile(int fileId)
        {
            var file = await _fileRepository.GetFileById(fileId);
            if (file == null) throw new Exception("File not found.");

            await _googleDriveService.DeleteFileFromDrive(file.FileUrl);
            await _fileRepository.DeleteFile(fileId);
        }
    }
}
