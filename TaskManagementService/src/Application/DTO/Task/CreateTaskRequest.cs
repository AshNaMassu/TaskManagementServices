using System.Text.Json.Serialization;

namespace Application.DTO.Task
{
    /// <summary>
    /// Запрос на создание задачи
    /// Пример:
    /// <code>
    /// {
    ///     "title": "Новая задача",
    ///     "description": "Подробное описание задачи"
    /// }
    /// </code>
    /// </summary>
    public class CreateTaskRequest
    {
        /// <summary>
        /// Название задачи (обязательное поле)
        /// </summary>
        /// <example>Новая задача</example>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Описание задачи
        /// </summary>
        /// <example>Подробное описание задачи</example>
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
