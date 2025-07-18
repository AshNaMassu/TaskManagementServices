using Infrastructure.Configurations;

namespace Infrastructure.Interfaces
{
    public interface IMessageConsumerService<TKafkaConfiguration, TConsumerConfiguration> : IDisposable
        where TKafkaConfiguration : KafkaConfigurationBase
        where TConsumerConfiguration : ConsumerConfigurationBase
    {
        Task<T> ConsumeAsync<T>();
        Task CommitDataAsync();
    }
}
