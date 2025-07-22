using Application.DTO.HealthCheck;

namespace Application.Interfaces
{
    /// <summary>
    /// Сервис проверки здоровья приложения
    /// </summary>
    public interface IHealthCheckService
    {
        /// <summary>
        /// Выполняет комплексную проверку здоровья сервиса
        /// </summary>
        /// <returns>Результат проверки</returns>
        Task<HealthCheck> CheckHealthAsync();
    }
}
