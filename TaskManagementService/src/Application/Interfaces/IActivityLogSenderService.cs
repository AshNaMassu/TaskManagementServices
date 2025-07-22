using Application.DTO.Logging;

namespace Application.Interfaces
{
    /// <summary>
    /// Сервис для отправки сообщений о изменениях в системе
    /// </summary>
    public interface IActivityLogSenderService
    {
        /// <summary>
        /// Отправляет сообщение о изменении сущности
        /// </summary>
        /// <param name="message">Данные о изменении</param>
        public Task SendMessage(EntityChangedMessage message);
    }
}
