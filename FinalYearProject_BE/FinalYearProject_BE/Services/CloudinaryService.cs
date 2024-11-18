using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Services.IService;
using FinalYearProject_BE.Settings;
using Microsoft.Extensions.Options;

namespace FinalYearProject_BE.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<UpdateUserDTO> UploadUserImage(IFormFile file)
        {
            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return new UpdateUserDTO
                {
                    ImageUrl = uploadResult?.SecureUrl.ToString(),
                };
            }

            throw new Exception("Invalid image file.");
        }

        public async Task<string> UploadCourseImage(IFormFile file)
        {
            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                var courseImage = uploadResult?.SecureUrl.ToString();

                return courseImage;
            }

            throw new Exception("Invalid image file.");
        }

        public async Task<string> UploadLessonVideo(IFormFile video)
        {
            if (video == null || video.Length == 0)
                throw new ArgumentException("Invalid video file.");

            await using var stream = video.OpenReadStream();

            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(video.FileName, stream),
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.AbsoluteUri;
            }
            else
            {
                throw new Exception($"Video upload failed: {uploadResult.Error?.Message}");
            }
        }

        public async Task DeleteFile(string fileUrl)
        {
            try
            {
                var publicId = ExtractPublicId(fileUrl);

                ResourceType resourceType;
                if (IsVideoFile(fileUrl))
                {
                    resourceType = ResourceType.Video;
                }
                else if (IsImageFile(fileUrl))
                {
                    resourceType = ResourceType.Image;
                }
                else
                {
                    throw new ArgumentException("Unsupported file type. Only image and video files are allowed.");
                }

                var deletionParams = new DeletionParams(publicId)
                {
                    ResourceType = resourceType
                };

                await _cloudinary.DestroyAsync(deletionParams);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }
        }

        private string ExtractPublicId(string fileUrl)
        {
            var uri = new Uri(fileUrl);
            var path = uri.AbsolutePath;
            var parts = path.Split('/');

            if (parts.Length > 2)
            {
                var fileNameWithExtension = parts[^1];
                var publicId = fileNameWithExtension.Substring(0, fileNameWithExtension.LastIndexOf('.')); // Bỏ đuôi file
                return publicId;
            }

            throw new ArgumentException("Invalid file URL. Cannot extract PublicId.");
        }

        private bool IsVideoFile(string fileUrl)
        {
            string[] videoExtensions = { ".mp4", ".mov", ".avi", ".mkv", ".flv", ".wmv" };

            var extensionIndex = fileUrl.LastIndexOf('.');
            if (extensionIndex != -1)
            {
                var fileExtension = fileUrl.Substring(extensionIndex).ToLower();
                return Array.Exists(videoExtensions, ext => ext == fileExtension);
            }

            return false;
        }

        private bool IsImageFile(string fileUrl)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

            var extensionIndex = fileUrl.LastIndexOf('.');
            if (extensionIndex != -1)
            {
                var fileExtension = fileUrl.Substring(extensionIndex).ToLower();
                return Array.Exists(imageExtensions, ext => ext == fileExtension);
            }

            return false;
        }
    }
}
