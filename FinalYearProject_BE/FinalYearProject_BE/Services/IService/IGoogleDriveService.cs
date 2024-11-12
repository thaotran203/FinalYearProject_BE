namespace FinalYearProject_BE.Services.IService
{
    public interface IGoogleDriveService
    {
        Task<string> UploadFileToDrive(IFormFile file, string fileName);
        Task DeleteFileFromDrive(string fileUrl);
    }
}
