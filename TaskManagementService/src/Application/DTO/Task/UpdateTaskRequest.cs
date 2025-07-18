using System.Text.Json.Serialization;

namespace Application.DTO.Task
{
    public class UpdateTaskRequest
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
