using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Services.IService;
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
    }
}
