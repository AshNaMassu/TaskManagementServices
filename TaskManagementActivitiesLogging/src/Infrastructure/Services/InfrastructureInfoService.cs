using Application.DTO.HealthCheck;
using Application.Enums;
using Application.Interfaces;
using Infrastructure.Confuguration;
using Infrastructure.Interfaces;

namespace Infrastructure.Services
{
    public class InfrastructureInfoService : IInfrastructureInfoService
    {
        private readonly IKafkaInfoService<KafkaConfigurationOptions, ActivityLogConsumerOptions> _kafkaInfoService;

        public InfrastructureInfoService(
            IKafkaInfoService<KafkaConfigurationOptions, ActivityLogConsumerOptions> kafkaInfoService)
        {
            _kafkaInfoService = kafkaInfoService;
        }

        public async Task<HealthCheck> HealthCheckAsync()
        {
            var (result, message) = await _kafkaInfoService.HealthCheckAsync();

            if (result)
            {
                return new HealthCheck
                {
                    Status = HealthCheckStatus.Ok
                };
            }
            else
            {
                return new HealthCheck
                {
                    Status = HealthCheckStatus.Failed,
                    Message = message
                };
            }
        }
    }
}