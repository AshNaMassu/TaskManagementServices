using Domain.Enums;
using System.Text.Json.Serialization;

namespace Application.DTO.Task
{
    /// <summary>
    /// Параметры фильтрации и пагинации списка задач
    /// </summary>
    /// <remarks>
    /// Все параметры являются необязательными. При отсутствии параметров возвращаются все задачи с пагинацией.
    /// 
    /// Пример запроса:
    /// <code>
    /// {
    ///     "title": "важн",
    ///     "status": "InProcess",
    ///     "created_at_start": "2023-01-01",
    ///     "limit": 50
    /// }
    /// </code>
    /// </remarks>
    public class FilteringTaskRequest
    {
        /// <summary>
        /// Список идентификаторов необходимых задач
        /// </summary>
        [JsonPropertyName("ids")]
        public long[]? Ids { get; set; }

        /// <summary>
        /// Фильтр по частичному совпадению названия задачи
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Фильтр по частичному совпадению описания задачи
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Фильтр по статусу
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        /// <summary>
        /// Начальная дата для фильтрации по дате создания (включительно)
        /// </summary>
        [JsonPropertyName("created_at_start")]
        public DateTime? CreatedAtStart { get; set; }

        /// <summary>
        /// Конечная дата для фильтрации по дате создания (включительно)
        /// </summary>
        [JsonPropertyName("created_at_end")]
        public DateTime? CreatedAtEnd { get; set; }

        /// <summary>
        /// Начальная дата для фильтрации по дате создания (включительно)
        /// </summary>
        [JsonPropertyName("updated_at_start")]
        public DateTime? UpdatedAtStart { get; set; }

        /// <summary>
        /// Конечная дата для фильтрации по дате создания (включительно)
        /// </summary>
        [JsonPropertyName("updated_at_end")]
        public DateTime? UpdatedAtEnd { get; set; }

        /// <summary>
        /// Максимальное количество возвращаемых записей (по умолчанию 50)
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; } = 50;

        /// <summary>
        /// Смещение для пагинации (по умолчанию 0) 
        /// </summary>
        [JsonPropertyName("offset")]
        public int Offset { get; set; }
    }
}
