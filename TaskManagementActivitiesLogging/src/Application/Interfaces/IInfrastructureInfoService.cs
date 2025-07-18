using Application.DTO.HealthCheck;

namespace Application.Interfaces
{
    public interface IInfrastructureInfoService
    {
        Task<HealthCheck> HealthCheckAsync();
    }
}
