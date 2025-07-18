using Confluent.Kafka;
using Infrastructure.Interfaces;

namespace Infrastructure.Configurations
{
    public abstract class ProducerConfigurationBase : ProducerConfig, ITopicConfiguration
    {
        public string Topic { get; set; }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(Topic);
            }
        }
    }
}
