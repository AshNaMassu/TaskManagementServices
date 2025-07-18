using System.Text.Json.Serialization;

namespace Application.DTO.Task
{
    public class UpdateTaskStatusRequest
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
