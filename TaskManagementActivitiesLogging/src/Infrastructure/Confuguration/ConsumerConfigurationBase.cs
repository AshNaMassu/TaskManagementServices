using Confluent.Kafka;
using Infrastructure.Interfaces;

namespace Infrastructure.Configurations
{
    public abstract class ConsumerConfigurationBase : ConsumerConfig, ITopicConfiguration
    {
        public string Topic { get; set; }
        public int RetryOnEmptyDelayMs { get; set; }
        public int RetryOnFailedDelayMs { get; set; } 
        public int ConsumeTimeoutMs { get; set; }
        public new bool EnableAutoCommit { get; set; } = false;

        public bool IsValid
        {
            get
            {
                return !(string.IsNullOrEmpty(Topic) ||
                         string.IsNullOrEmpty(ClientId) ||
                         string.IsNullOrEmpty(GroupId) ||
                         RetryOnEmptyDelayMs == 0 ||
                         RetryOnFailedDelayMs == 0 ||
                         ConsumeTimeoutMs == 0);
            }
        }
    }
}
