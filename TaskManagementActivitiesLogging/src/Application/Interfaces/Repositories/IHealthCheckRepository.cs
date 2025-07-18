using Application.DTO.HealthCheck;

namespace Application.Interfaces.Repositories
{
    public interface IHealthCheckRepository
    {
        Task<HealthCheck> CheckHealthDbAsync();
    }
}
