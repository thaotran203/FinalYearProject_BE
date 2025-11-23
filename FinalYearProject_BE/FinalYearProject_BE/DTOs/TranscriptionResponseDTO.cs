using System.Text.Json.Serialization;

namespace FinalYearProject_BE.DTOs
{
    public class TranscriptionResponseDTO
    {
        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("duration")]
        public float Duration { get; set; }

        [JsonPropertyName("segments")]
        public List<SegmentDTO> Segments { get; set; }
    }
}
