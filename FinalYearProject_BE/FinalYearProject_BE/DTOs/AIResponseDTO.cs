namespace FinalYearProject_BE.DTOs
{
    public class AIResponseDTO
    {
        // DTO cho response từ endpoint /transcribe
        public record TranscriptionResponse(List<Segment> Segments);
        public record Segment(int Id, float Start, float End, string Text);

        // DTO cho response từ endpoint /embed
        public record EmbeddingResponse(string Text, List<float> Vector);
    }
}
