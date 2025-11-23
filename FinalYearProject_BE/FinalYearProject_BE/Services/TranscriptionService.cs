using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Repository.IRepository;
using FinalYearProject_BE.Services.IService;
using System.Text;
using static FinalYearProject_BE.DTOs.AIResponseDTO;

namespace FinalYearProject_BE.Services
{
    public class TranscriptionService : ITranscriptionService
    {
        private readonly ILessonVideoRepository _videoRepository;
        private readonly HttpClient _httpClient;
        private readonly ILogger<TranscriptionService> _logger;
        private readonly string _aiServiceUrl;

        public TranscriptionService(
            ILessonVideoRepository videoRepository,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<TranscriptionService> logger)
        {
            _videoRepository = videoRepository;
            _logger = logger;
            _aiServiceUrl = configuration["ServiceUrls:AIService"]!;

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(15);
        }

        public async Task GenerateTranscriptAsync(int lessonVideoId)
        {
            var video = await _videoRepository.GetVideoById(lessonVideoId);
            if (video == null)
            {
                _logger.LogError("Video not found with ID: {VideoId}", lessonVideoId);
                return;
            }

            try
            {
                video.ProcessingStatus = "Processing";
                await _videoRepository.UpdateVideo(video);

                _logger.LogInformation("Starting transcription for video ID: {VideoId}", lessonVideoId);

                // BƯỚC 1: LẤY TRANSCRIPT
                var transRequest = new { video_url = video.VideoUrl };
                var response = await _httpClient.PostAsJsonAsync($"{_aiServiceUrl}/transcribe", transRequest);
                response.EnsureSuccessStatusCode();

                var transResponse = await response.Content.ReadFromJsonAsync<TranscriptionResponse>();
                if (transResponse == null || transResponse.Segments.Count == 0)
                {
                    throw new InvalidOperationException("AI service returned no segments.");
                }
                _logger.LogInformation("Transcription successful. Found {SegmentCount} segments.", transResponse.Segments.Count);

                // BƯỚC 2: LƯU FULL TRANSCRIPT VÀO SQL
                var transcriptBuilder = new StringBuilder();
                foreach (var segment in transResponse.Segments)
                {
                    var startTime = TimeSpan.FromSeconds(segment.Start).ToString(@"hh\:mm\:ss");
                    transcriptBuilder.AppendLine($"{startTime} - {segment.Text.Trim()}");
                }
                video.Transcript = transcriptBuilder.ToString();

                // BƯỚC 3: HOÀN TẤT
                video.ProcessingStatus = "Completed";
                _logger.LogInformation("Successfully processed video ID: {VideoId}", lessonVideoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process video ID: {VideoId}", lessonVideoId);
                video.ProcessingStatus = "Failed";
                video.ErrorMessage = ex.Message;
            }
            finally
            {
                await _videoRepository.UpdateVideo(video);
            }
        }
    }
}