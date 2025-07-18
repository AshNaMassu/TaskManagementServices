using System.Text.Json.Serialization;

namespace Application.DTO.ActivityLog
{
    public class FilteringActivityLogRequest
    {
        [JsonPropertyName("ids")]
        public long[]? Ids { get; set; }

        [JsonPropertyName("event_type")]
        public string? EventType { get; set; }

        [JsonPropertyName("entity")]
        public string? Entity { get; set; }

        [JsonPropertyName("entity_id")]
        public long? EntityId { get; set; }

        /// <summary>
        /// Начальная дата для поиска по интервалу даты события
        /// </summary>
        [JsonPropertyName("event_time_start")]
        public DateTime? EventTimeStart { get; set; }

        /// <summary>
        /// Конечная дата для поиска по интервалу даты события
        /// </summary>
        [JsonPropertyName("event_time_end")]
        public DateTime? EventTimeEnd { get; set; }
    }
}
