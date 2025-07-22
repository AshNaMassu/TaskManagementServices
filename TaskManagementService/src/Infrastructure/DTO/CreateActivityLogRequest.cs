using System.Text.Json.Serialization;

namespace Infrastructure.DTO
{
    /// <summary>
    /// Модель запроса для создания лога активности
    /// </summary>
    public class CreateActivityLogRequest
    {
        /// <summary>
        /// Тип события
        /// </summary>
        [JsonPropertyName("event_type")]
        public string EventType { get; set; }

        /// <summary>
        /// Тип сущности
        /// </summary>
        [JsonPropertyName("entity")]
        public string Entity { get; set; }

        /// <summary>
        /// ID изменяемой сущности
        /// </summary>
        [JsonPropertyName("entity_id")]
        public long EntityId { get; set; }

        // <summary>
        /// Время события в UTC
        /// </summary>
        [JsonPropertyName("event_time")]
        public DateTime EventTime { get; set; }
    }
}
