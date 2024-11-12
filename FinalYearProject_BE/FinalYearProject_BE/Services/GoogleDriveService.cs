using FinalYearProject_BE.Services.IService;
using FinalYearProject_BE.Settings;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Options;

namespace FinalYearProject_BE.Services
{
    public class GoogleDriveService : IGoogleDriveService
    {
        private readonly GoogleDriveSettings _settings;
        private readonly DriveService _driveService;

        public GoogleDriveService(IOptions<GoogleDriveSettings> settings)
        {
            _settings = settings.Value;

            // Initialize the Google Drive service using OAuth2 credentials
            var credential = GoogleCredential.FromFile(_settings.CredentialsPath)
                .CreateScoped(DriveService.ScopeConstants.Drive);

            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "FinalYearProject"
            });
        }

        public async Task<string> UploadFileToDrive(IFormFile file, string fileName)
        {
            if (file.Length > 0)
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File
                {
                    Name = fileName,
                    Parents = new List<string> { "1rVopkL7IXiFcTiEzXM_7C7nkyegqy0XA" }
                };

                await using var stream = file.OpenReadStream();
                var request = _driveService.Files.Create(fileMetadata, stream, file.ContentType);
                request.Fields = "id";
                var result = await request.UploadAsync();

                if (result.Status != Google.Apis.Upload.UploadStatus.Failed)
                {
                    return $"https://drive.google.com/file/d/{request.ResponseBody.Id}/view";
                }
            }
            throw new Exception("File upload failed.");
        }

        public async Task DeleteFileFromDrive(string fileUrl)
        {
            var fileId = ExtractFileIdFromUrl(fileUrl);
            if (string.IsNullOrEmpty(fileId))
            {
                throw new ArgumentException("Invalid file URL.");
            }

            try
            {
                await _driveService.Files.Delete(fileId).ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("File deletion from Google Drive failed.", ex);
            }
        }

        private string ExtractFileIdFromUrl(string fileUrl)
        {
            var uri = new Uri(fileUrl);
            var segments = uri.Segments;
            var fileIdSegment = segments.FirstOrDefault(s => s != "/" && s != "file/" && s != "d/" && s != "view");
            return fileIdSegment?.Trim('/');
        }
    }
}
