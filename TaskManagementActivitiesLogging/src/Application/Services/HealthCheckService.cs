using Application.DTO.HealthCheck;
using Application.Enums;
using Application.Interfaces.Repositories;

namespace Application.Interfaces
{
    public class HealthCheckService : IHealthCheckService
    {
        private readonly IHealthCheckRepository _healthCheckRepository;
        private readonly IInfrastructureInfoService _infrastructureInfoService;

        public HealthCheckService(IHealthCheckRepository healthCheckRepository, IInfrastructureInfoService infrastructureInfoService)
        {
            _healthCheckRepository = healthCheckRepository;
            _infrastructureInfoService = infrastructureInfoService;
        }

        public async Task<HealthCheck> CheckHealthAsync()
        {
            var infraHealthCheck = await _infrastructureInfoService.HealthCheckAsync();

            if (infraHealthCheck.Status != HealthCheckStatus.Ok)
            {
                return infraHealthCheck;
            }

            var dbHealthCheck = await _healthCheckRepository.CheckHealthDbAsync();
            return dbHealthCheck;
        }
    }
}
