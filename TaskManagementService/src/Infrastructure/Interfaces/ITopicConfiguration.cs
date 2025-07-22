namespace Infrastructure.Interfaces
{
    /// <summary>
    /// Конфигурация топика сообщений
    /// </summary>
    public interface ITopicConfiguration
    {
        /// <summary>
        /// Название топика
        /// </summary>
        string Topic { get; set; }
    }
}
