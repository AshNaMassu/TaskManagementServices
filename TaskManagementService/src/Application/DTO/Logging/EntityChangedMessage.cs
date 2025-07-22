namespace Application.DTO.Logging
{
    /// <summary>
    /// Информация об изменении сущности
    /// </summary>
    public class EntityChangedMessage
    {
        /// <summary>
        /// Выполненная операция
        /// </summary>
        public string Operation { get; set; }
        
        /// <summary>
        /// Название изменившейся сущности
        /// </summary>
        public string Entity { get; set; }
        
        /// <summary>
        /// Идентификатор изменившийся сущности
        /// </summary>
        public long EntityId { get; set; }
    }
}
