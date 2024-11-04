using FinalYearProject_BE.DTOs;

namespace FinalYearProject_BE.Services.IService
{
    public interface ICloudinaryService
    {
        Task<UpdateUserDTO> UploadImage(IFormFile file);
    }
}
