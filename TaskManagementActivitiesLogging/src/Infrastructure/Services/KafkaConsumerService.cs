using Confluent.Kafka;
using Infrastructure.Configurations;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class KafkaConsumerService<TKafkaConfiguration, TConsumerConfiguration> : IMessageConsumerService<TKafkaConfiguration, TConsumerConfiguration>
        where TKafkaConfiguration : KafkaConfigurationBase
        where TConsumerConfiguration : ConsumerConfigurationBase
    {
        private readonly TConsumerConfiguration _consumerConfig;
        private IConsumer<Ignore, string> _consumer;
        private readonly ILogger<KafkaConsumerService<TKafkaConfiguration, TConsumerConfiguration>> _logger;
        private ConsumeResult<Ignore, string>? consumedMessage;

        public KafkaConsumerService(
            IOptions<TKafkaConfiguration> kafkaConfig,
            IOptions<TConsumerConfiguration> consumerConfig,
            ILogger<KafkaConsumerService<TKafkaConfiguration, TConsumerConfiguration>> logger
            )
        {
            _consumerConfig = consumerConfig.Value;
            _logger = logger;

            SetKafkaValues(_consumerConfig, kafkaConfig.Value);

            _consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();

            _consumer.Subscribe(_consumerConfig.Topic);
        }

        private void SetKafkaValues(TConsumerConfiguration consumerConfig, TKafkaConfiguration kafkaConfig)
        {
            consumerConfig.BootstrapServers = kafkaConfig.BootstrapServers;
            consumerConfig.SaslUsername = kafkaConfig.SaslUsername;
            consumerConfig.SaslPassword = kafkaConfig.SaslPassword;
            consumerConfig.SaslMechanism = kafkaConfig.SaslMechanism;
            consumerConfig.SecurityProtocol = kafkaConfig.SecurityProtocol;
        }

        public async Task<T> ConsumeAsync<T>()
        {
            consumedMessage = null;
            while (true)
            {
                _logger.LogDebug("[{topic}] Получение нового сообщения", _consumerConfig.Topic);

                try
                {
                    consumedMessage = _consumer.Consume(_consumerConfig.ConsumeTimeoutMs);
                }
                catch (Exception ex)
                {
                    _logger.LogError("[{topic}] Не удалось получить новое сообщение: {err}. Следующая попытка через {delay} секунд",
                        _consumerConfig.Topic, ex.Message, _consumerConfig.RetryOnFailedDelayMs / 1000);
                    await Task.Delay(_consumerConfig.RetryOnFailedDelayMs);
                    continue;
                }

                if (consumedMessage == null)
                {
                    _logger.LogDebug("[{topic}] Нет новых сообщений", _consumerConfig.Topic);
                    await Task.Delay(_consumerConfig.RetryOnEmptyDelayMs);
                }
                else
                {
                    _logger.LogInformation("Получено сообщение (Offset = {offset}, TopicPartitionOffset = {topic_partition_offset}):{new_line}{value}",
                        consumedMessage.Offset, consumedMessage.TopicPartitionOffset, Environment.NewLine, consumedMessage.Message.Value);

                    return JsonSerializer.Deserialize<T>(consumedMessage.Message.Value);
                }
            }
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }

        public async Task CommitDataAsync()
        {
            if (consumedMessage == null)
            {
                return;
            }

            while (true)
            {
                try
                {
                    _logger.LogDebug("[{topic}] Выполняется коммит", _consumerConfig.Topic);
                    _consumer.Commit(consumedMessage);
                    _logger.LogDebug("[{topic}] Коммит выполнен успешно", _consumerConfig.Topic);
                    consumedMessage = null;
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError("[{topic}] Не удалось выполнить коммит: {err}. Следующая попытка через {delay} секунд",
                        _consumerConfig.Topic, ex.Message, _consumerConfig.RetryOnFailedDelayMs / 1000);
                    await Task.Delay(_consumerConfig.RetryOnFailedDelayMs);
                }
            }
        }
    }
}
