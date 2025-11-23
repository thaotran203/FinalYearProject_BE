namespace FinalYearProject_BE.Services.IService
{
    public interface ITranscriptionService
    {
        Task GenerateTranscriptAsync(int lessonVideoId);
    }
}
