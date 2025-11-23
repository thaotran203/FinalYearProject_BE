using System.Text.Json.Serialization;

namespace FinalYearProject_BE.DTOs
{
    public class SegmentDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("start")]
        public float Start { get; set; }

        [JsonPropertyName("end")]
        public float End { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
