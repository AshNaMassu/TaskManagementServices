using System.Text.Json.Serialization;

namespace Application.DTO.Task
{
    /// <summary>
    /// Модель запроса на обновление статуса задачи
    /// </summary>
    public class UpdateTaskStatusRequest
    {
        /// <summary>
        /// Новый статус задачи
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
