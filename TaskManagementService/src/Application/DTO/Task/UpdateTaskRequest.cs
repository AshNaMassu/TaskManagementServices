using System.Text.Json.Serialization;

namespace Application.DTO.Task
{
    /// <summary>
    /// Модель запроса на обновление задачи
    /// </summary>
    public class UpdateTaskRequest
    {
        /// <summary>
        /// Новое название задачи   
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Новое описание
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Новый статус задачи
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
