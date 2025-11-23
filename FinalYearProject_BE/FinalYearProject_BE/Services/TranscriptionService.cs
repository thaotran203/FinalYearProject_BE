using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Repository.IRepository;
using FinalYearProject_BE.Services.IService;
using Qdrant.Client.Grpc;
using System.Text;
using static FinalYearProject_BE.DTOs.AIResponseDTO;
using Qdrant.Client;
using Qdrant.Client.Grpc;


namespace FinalYearProject_BE.Services
{
    public class TranscriptionService : ITranscriptionService
    {
        private readonly ILessonVideoRepository _videoRepository;
        private readonly HttpClient _httpClient;
        private readonly ILogger<TranscriptionService> _logger;
        private readonly QdrantClient _qdrantClient;
        private readonly string _aiServiceUrl;

        private const string QdrantCollectionName = "video_segments";
        private const uint VectorSize = 384;

        public TranscriptionService(
            ILessonVideoRepository videoRepository,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<TranscriptionService> logger)
        {
            _videoRepository = videoRepository;
            _logger = logger;

            _aiServiceUrl = configuration["ServiceUrls:AIService"]!;

            _logger.LogInformation("ĐANG TEST: Kết nối tới Qdrant tại localhost:6334 (dùng constructor Host/Port)");
            try
            {
                _qdrantClient = new QdrantClient(host: "localhost", port: 6334);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LỖI ngay cả khi dùng constructor Host/Port");
                throw;
            }

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(15);
        }

        private async Task EnsureQdrantCollectionExistsAsync()
        {
            var collections = await _qdrantClient.ListCollectionsAsync();
            if (!collections.Contains(QdrantCollectionName))
            {
                _logger.LogInformation("Qdrant collection '{CollectionName}' not found. Creating...", QdrantCollectionName);
                await _qdrantClient.CreateCollectionAsync(
                    collectionName: QdrantCollectionName,
                    vectorsConfig: new VectorParams { Size = VectorSize, Distance = Distance.Cosine }
                );
                _logger.LogInformation("Qdrant collection created.");
            }
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
                _logger.LogInformation("Starting transcription & vectorization for video ID: {VideoId}", lessonVideoId);

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

                // BƯỚC 3: VECTOR HÓA VÀ LƯU VÀO QDRANT

                await EnsureQdrantCollectionExistsAsync();

                var pointsToUpsert = new List<PointStruct>();

                foreach (var segment in transResponse.Segments)
                {
                    var embedRequest = new { text = segment.Text.Trim() };
                    var embedResponse = await _httpClient.PostAsJsonAsync($"{_aiServiceUrl}/embed", embedRequest);

                    if (!embedResponse.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("Failed to embed segment: {SegmentText}", segment.Text);
                        continue;
                    }

                    var embedding = await embedResponse.Content.ReadFromJsonAsync<EmbeddingResponse>();
                    if (embedding == null || embedding.Vector.Count == 0) continue;

                    var point = new PointStruct
                    {
                        Id = new PointId { Uuid = Guid.NewGuid().ToString() },
                        Vectors = embedding.Vector.ToArray(),

                        Payload =
                        {
                            ["videoId"] = video.Id,
                            ["lessonId"] = video.LessonId,
                            ["text"] = segment.Text.Trim(),
                            ["startTime"] = segment.Start,
                            ["endTime"] = segment.End
                        }
                    };
                    pointsToUpsert.Add(point);
                }

                if (pointsToUpsert.Count > 0)
                {
                    await _qdrantClient.UpsertAsync(
                            collectionName: QdrantCollectionName,
                            points: pointsToUpsert
                        );
                    _logger.LogInformation("Successfully vectorized and saved {PointCount} segments to Qdrant.", pointsToUpsert.Count);
                }

                // BƯỚC 4: HOÀN TẤT
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