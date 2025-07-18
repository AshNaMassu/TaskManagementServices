using Confluent.Kafka;
using Infrastructure.Configurations;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class KafkaInfoService<TKafkaConfiguration, TConfigTopic> : IKafkaInfoService<TKafkaConfiguration, TConfigTopic>
        where TKafkaConfiguration : KafkaConfigurationBase
        where TConfigTopic : class, ITopicConfiguration
    {
        private readonly TKafkaConfiguration _kafkaConfig;
        private readonly TConfigTopic _configTopic;

        public KafkaInfoService(
            IOptions<TKafkaConfiguration> kafkaConfig,
            IOptions<TConfigTopic> topicOptions)
        {
            _kafkaConfig = kafkaConfig.Value;
            _configTopic = topicOptions.Value;
        }

        public async Task<(bool, string)> HealthCheckAsync()
        {
            return await CheckKafka();
        }

        private async Task<(bool, string)> CheckKafka()
        {
            return await Task.Run(() =>
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = _kafkaConfig.BootstrapServers,
                    SaslUsername = _kafkaConfig.SaslUsername,
                    SaslPassword = _kafkaConfig.SaslPassword,
                    SaslMechanism = _kafkaConfig.SaslMechanism,
                    SecurityProtocol = _kafkaConfig.SecurityProtocol,
                    SocketTimeoutMs = 5000
                };

                using var adminClient = new AdminClientBuilder(config).Build();
                Metadata? metadata = null;

                try
                {
                    metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));
                }
                catch (Exception ex)
                {
                    return (false, $"Connection failed to Kafka address '{_kafkaConfig.BootstrapServers}'");
                }

                var missingTopics = new List<string>();

                var topicMetadata = metadata.Topics.FirstOrDefault(t => t.Topic == _configTopic.Topic);
                if (topicMetadata == null || topicMetadata.Error.IsError)
                {
                    return (false, $"Topic '{_configTopic.Topic}' of '{typeof(TConfigTopic).Name}' for Kafka address '{_kafkaConfig.BootstrapServers}' is not available");
                }

                return (true, string.Empty);
            });
        }
    }
}