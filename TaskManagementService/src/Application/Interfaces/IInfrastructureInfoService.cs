using Application.DTO.HealthCheck;

namespace Application.Interfaces
{
    /// <summary>
    /// Сервис для проверки состояния инфраструктуры
    /// </summary>
    /// <remarks>Отличается от IHealthCheckService более глубокой проверкой инфраструктурных компонентов</remarks>
    public interface IInfrastructureInfoService
    {
        /// <summary>
        /// Выполняет проверку состояния инфраструктуры
        /// </summary>
        Task<HealthCheck> HealthCheckAsync();
    }
}
