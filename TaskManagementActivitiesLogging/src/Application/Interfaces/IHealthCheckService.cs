using Application.DTO.HealthCheck;

namespace Application.Interfaces
{
    public interface IHealthCheckService
    {
        Task<HealthCheck> CheckHealthAsync();
    }
}
