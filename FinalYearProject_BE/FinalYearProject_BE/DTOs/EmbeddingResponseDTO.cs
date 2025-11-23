using System.Text.Json.Serialization;

namespace FinalYearProject_BE.DTOs
{
    public class EmbeddingResponseDTO
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("vector")]
        public List<float> Vector { get; set; }
    }
}
