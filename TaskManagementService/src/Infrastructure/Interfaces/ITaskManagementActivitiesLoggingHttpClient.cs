using Infrastructure.DTO;
using Refit;

namespace Infrastructure.Interfaces
{
    /// <summary>
    /// HTTP-клиент для сервиса логирования активности
    /// </summary>
    public interface ITaskManagementActivitiesLoggingHttpClient
    {
        /// <summary>
        /// Отправляет лог активности в сервис логирования
        /// </summary>
        /// <param name="request">Данные лога</param>
        /// <returns>API-ответ</returns>
        [Post("/api/v1/activity-log")]
        Task<IApiResponse> CreateAsync(CreateActivityLogRequest request);
    }
}
