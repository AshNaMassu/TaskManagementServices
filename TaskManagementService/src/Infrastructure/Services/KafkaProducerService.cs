using Application.Interfaces;
using Confluent.Kafka;
using Domain.Enums;
using Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class KafkaProducerService<TKafkaConfiguration, TProducerConfiguration> : IDisposable, IEventBusPublisher<TProducerConfiguration>
        where TKafkaConfiguration : KafkaConfigurationBase
        where TProducerConfiguration : ProducerConfigurationBase
    {
        private JsonSerializerOptions _serializerOptions;
        private readonly ILogger<KafkaProducerService<TKafkaConfiguration, TProducerConfiguration>> _logger;
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public KafkaProducerService(
            IOptions<TKafkaConfiguration> kafkaConfig,
            IOptions<TProducerConfiguration> producerConfig,
            ILogger<KafkaProducerService<TKafkaConfiguration, TProducerConfiguration>> logger)
        {
            _logger = logger;
            _topic = producerConfig.Value.Topic;

            SetKafkaValues(producerConfig.Value, kafkaConfig.Value);

            _producer = new ProducerBuilder<Null, string>(producerConfig.Value)
                .Build();

            _serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        private void SetKafkaValues(TProducerConfiguration producerConfig, TKafkaConfiguration kafkaConfig)
        {
            producerConfig.BootstrapServers = kafkaConfig.BootstrapServers;
            producerConfig.SaslUsername = kafkaConfig.SaslUsername;
            producerConfig.SaslPassword = kafkaConfig.SaslPassword;
            producerConfig.SaslMechanism = kafkaConfig.SaslMechanism;
            producerConfig.SecurityProtocol = kafkaConfig.SecurityProtocol;
        }

        public void Dispose()
        {
            _producer.Dispose();
        }

        public async Task PublishAsync<T>(T message) where T : class
        {
            string messageText = JsonSerializer.Serialize(message, _serializerOptions);

            _logger.LogInformation($"[{_topic}] Отправляется сообщение {Environment.NewLine}{messageText}");

            try
            {
                var result = await _producer.ProduceAsync(_topic, new Message<Null, string>() { Value = messageText });
                DeliveryResultHandler(result);
            }
            catch (ProduceException<Null, string> pe)
            {
                var error = $"[{_topic}] Ошибка доставки. Причина: {pe.Error.Reason}";
                _logger.LogError(pe, error);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[{_topic}] Критическая ошибка при отправке сообщения: {ex.Message}" );
                throw;
            }

            _logger.LogInformation($"[{_topic}] Сообщение отправлено успешно");
        }

        private void DeliveryResultHandler(DeliveryResult<Null, string> result)
        {
            switch (result.Status)
            {
                case PersistenceStatus.Persisted:
                    _logger.LogInformation($"[{_topic}] Сообщение успешно доставлено (Offset: {result.Offset})");
                    break;

                case PersistenceStatus.PossiblyPersisted:
                    _logger.LogWarning($"[{_topic}] Сообщение возможно доставлено (требует проверки)");
                    break;

                case PersistenceStatus.NotPersisted:
                    throw new Exception($"[{_topic}] Сообщение не доставлено. Причина: {result.Message}");

                default:
                    throw new Exception($"[{_topic}] Неизвестный статус доставки: {result.Status}");
            }
        }
    }
}
