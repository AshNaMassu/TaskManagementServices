using Confluent.Kafka;

namespace Infrastructure.Configurations
{
    public abstract class KafkaConfigurationBase : ClientConfig
    {
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(BootstrapServers);
            }
        }
    }
}
