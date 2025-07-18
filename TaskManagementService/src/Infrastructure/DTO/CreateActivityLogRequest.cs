using System.Text.Json.Serialization;

namespace Infrastructure.DTO
{
    public class CreateActivityLogRequest
    {
        [JsonPropertyName("event_type")]
        public string EventType { get; set; }

        [JsonPropertyName("entity")]
        public string Entity { get; set; }

        [JsonPropertyName("entity_id")]
        public long EntityId { get; set; }

        [JsonPropertyName("event_time")]
        public DateTime EventTime { get; set; }
    }
}
