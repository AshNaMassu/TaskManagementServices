﻿using Domain.Enums;
using System.Text.Json.Serialization;

namespace Application.DTO.Task
{
    public class FilteringTaskRequest
    {
        [JsonPropertyName("ids")]
        public long[]? Ids { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("created_at_start")]
        public DateTime? CreatedAtStart { get; set; }

        [JsonPropertyName("created_at_end")]
        public DateTime? CreatedAtEnd { get; set; }

        [JsonPropertyName("updated_at_start")]
        public DateTime? UpdatedAtStart { get; set; }

        [JsonPropertyName("updated_at_end")]
        public DateTime? UpdatedAtEnd { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; } = 50;

        [JsonPropertyName("offsett")]
        public int Offset { get; set; }
    }
}
