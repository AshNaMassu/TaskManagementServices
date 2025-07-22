namespace Application.Interfaces
{
    /// <summary>
    /// Абстракция для публикации событий в брокер сообщений
    /// </summary>
    /// <typeparam name="TSettings">Тип настроек подключения</typeparam>
    public interface IEventBusPublisher<TSettings>
    {
        /// <summary>
        /// Отправляет сообщение в брокер
        /// </summary>
        /// <typeparam name="T">Тип сообщения</typeparam>
        /// <param name="message">Данные сообщения</param>
        Task PublishAsync<T>(T message) where T : class;
    }
}
