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

        /// <summary>
        /// Дата создания задачи (устанавливается автоматически)
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления задачи (устанавливается автоматически)
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}