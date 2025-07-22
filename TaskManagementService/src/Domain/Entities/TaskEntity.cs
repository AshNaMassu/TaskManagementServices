namespace Domain.Entities
{
    /// <summary>
    /// Сущность задачи пользователя
    /// </summary>
    public class TaskEntity
    {
        /// <summary>
        /// Уникальный идентификатор задачи
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Название задачи
        /// </summary>
        /// <example>Реализовать документирование API</example>
        public string Title { get; set; }

        /// <summary>
        /// Подробное описание задачи
        /// </summary>
        /// <example>Добавить XML-документацию для всех DTO и моделей</example>
        public string Description { get; set; }

        /// <summary>
        /// Текущий статус задачи
        /// </summary>
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}