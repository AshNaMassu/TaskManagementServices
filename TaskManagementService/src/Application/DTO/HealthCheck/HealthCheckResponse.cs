using System.Text.Json.Serialization;

namespace Application.DTO.HealthCheck
{
    public class HealthCheckResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
