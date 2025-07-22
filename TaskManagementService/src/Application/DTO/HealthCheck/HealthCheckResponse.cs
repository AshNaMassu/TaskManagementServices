using System.Text.Json.Serialization;

namespace Application.DTO.HealthCheck
{
    /// <summary>
    /// Ответ о состояния сервиса
    /// </summary>
    public class HealthCheckResponse
    {
        /// <summary>
        /// Текущий статус сервиса
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// Дополнительное сообщение о состоянии
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
