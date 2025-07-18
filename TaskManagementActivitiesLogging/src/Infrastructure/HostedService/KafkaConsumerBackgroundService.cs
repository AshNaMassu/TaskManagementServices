using Application.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.HostedService
{
    public class KafkaConsumerBackgroundService<TKafkaConfiguration, TConsumerConfiguration, TMessage> : BackgroundService
        where TKafkaConfiguration : KafkaConfigurationBase
        where TConsumerConfiguration : ConsumerConfigurationBase
        where TMessage : class
    {
        private readonly IMessageConsumerService<TKafkaConfiguration, TConsumerConfiguration> _messageConsumer;
        private readonly ILogger<KafkaConsumerBackgroundService<TKafkaConfiguration, TConsumerConfiguration, TMessage>> _logger;
        private readonly TConsumerConfiguration _consumerConfig;
        private readonly IServiceProvider _serviceProvider;

        public KafkaConsumerBackgroundService(
            IMessageConsumerService<TKafkaConfiguration, TConsumerConfiguration> messageConsumer,
            ILogger<KafkaConsumerBackgroundService<TKafkaConfiguration, TConsumerConfiguration, TMessage>> logger,
            IOptions<TConsumerConfiguration> consumerConfig,
            IServiceProvider serviceProvider)
        {
            _messageConsumer = messageConsumer;
            _logger = logger;
            _consumerConfig = consumerConfig.Value;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            await Task.WhenAll(ParseMessagesAsync(cancellationToken));
        }

        private async Task ParseMessagesAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                TMessage consumerMessage = default;
                bool resultMessageHandler = false;

                try
                {
                    consumerMessage = await _messageConsumer.ConsumeAsync<TMessage>();
                    resultMessageHandler = await ExecuteMessageHandlerAsync(consumerMessage);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ошибка при обработке сообщения топика {_consumerConfig.Topic}: {ex.Message}");
                }

                if (resultMessageHandler)
                {
                    await _messageConsumer.CommitDataAsync();
                }
                else
                {
                    _logger.LogError("[{topic}] Не удалось обработать сообщение. Следующая попытка через {delay} секунд",
                        _consumerConfig.Topic, _consumerConfig.RetryOnFailedDelayMs / 1000);
                    await Task.Delay(_consumerConfig.RetryOnFailedDelayMs);
                }
            }
        }

        private async ValueTask<bool> ExecuteMessageHandlerAsync(TMessage consumerMessage)
        {
            var messageHandler = _serviceProvider.GetRequiredService<IEventBusSubscriber<TMessage>>();
            return await messageHandler.Handle(consumerMessage);
        }
    }
}
