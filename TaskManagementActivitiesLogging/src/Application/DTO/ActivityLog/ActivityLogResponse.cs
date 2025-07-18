using System.Text.Json.Serialization;

namespace Application.DTO.ActivityLog
{
    public class ActivityLogResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

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
