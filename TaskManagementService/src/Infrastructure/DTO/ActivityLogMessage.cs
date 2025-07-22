using System.Text.Json.Serialization;

namespace Infrastructure.DTO
{
    // <summary>
    /// Сообщение о событии изменения задачи для Kafka
    /// </summary>
    public class ActivityLogMessage
    {
        /// <summary>
        /// Тип события, изменившего сущность(выполненная операция)
        /// </summary>
        /// <example>UpdateAsync</example>
        [JsonPropertyName("event_type")]
        public string EventType { get; set; }

        /// <summary>
        /// Название типа сущности
        /// </summary>
        /// <example>TaskEntity</example>
        [JsonPropertyName("entity")]
        public string Entity { get; set; }

        /// <summary>
        /// Идентификатор изменившейся сущности</summary>
        /// <example>12345</example>
        [JsonPropertyName("entity_id")]
        public long EntityId { get; set; }

        /// <summary>
        /// Время, в которое произошло изменение (UTC, устанавливается автоматически)
        /// </summary>
        [JsonPropertyName("event_time")]
        public DateTime EventTime { get; set; }
    }
}
