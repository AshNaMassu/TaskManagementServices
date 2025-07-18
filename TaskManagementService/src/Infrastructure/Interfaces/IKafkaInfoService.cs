using Infrastructure.Configurations;

namespace Infrastructure.Interfaces
{
    public interface IKafkaInfoService<TKafkaConfiguration, TConfigTopic>
        where TKafkaConfiguration : KafkaConfigurationBase
        where TConfigTopic : class, ITopicConfiguration
    {
        Task<(bool result, string errorMessage)> HealthCheckAsync();
    }
}
