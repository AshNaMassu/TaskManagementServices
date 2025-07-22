using Application.DTO.Logging;

namespace Application.Interfaces
{
    /// <summary>
    /// Сервис логирования изменений сущностей
    /// </summary>
    public interface IChangesLoggingService
    {
        /// <summary>
        /// Логирует изменение сущности
        /// </summary>
        /// <param name="message">Данные о изменении</param>
        Task Log(EntityChangedMessage message);
    }
}
