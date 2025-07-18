using System.Text.Json.Serialization;

namespace Application.DTO.Task
{
    public class CreateTaskRequest
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
