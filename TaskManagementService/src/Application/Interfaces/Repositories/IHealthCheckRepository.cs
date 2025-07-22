using Application.DTO.HealthCheck;

namespace Application.Interfaces.Repositories
{
    /// <summary>
    /// Репозиторий для проверки состояния БД
    /// </summary>
    public interface IHealthCheckRepository
    {
        // <summary>
        /// Проверяет доступность и работоспособность БД
        /// </summary>
        /// <returns>Результат проверки с статусом и сообщением</returns>
        Task<HealthCheck> CheckHealthDbAsync();
    }
}
