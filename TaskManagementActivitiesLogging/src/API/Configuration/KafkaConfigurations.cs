using Infrastructure.Configurations;
using Infrastructure.HostedService;
using Infrastructure.Interfaces;
using Infrastructure.Services;

namespace Infrastructure.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddKafka<TKafkaConfiguration>(this WebApplicationBuilder builder)
            where TKafkaConfiguration : KafkaConfigurationBase
        {
            var kafkaSectionName = typeof(TKafkaConfiguration).Name;

            builder.Services.Configure<TKafkaConfiguration>(builder.Configuration
                .GetSection(kafkaSectionName));

            var kafkaConfiguration = builder.Configuration
                .GetSection(kafkaSectionName)
                .Get<TKafkaConfiguration>();

            if (kafkaConfiguration is null || !kafkaConfiguration.IsValid)
            {
                throw new ArgumentException($"'{kafkaSectionName}' is not configured!");
            }

            return builder;
        }

        public static WebApplicationBuilder AddKafkaConsumer<TKafkaConfiguration, TConsumerConfiguration, TConsumerMessage>(this WebApplicationBuilder builder)
            where TKafkaConfiguration : KafkaConfigurationBase
            where TConsumerConfiguration : ConsumerConfigurationBase
            where TConsumerMessage : class
        {
            var kafkaSectionName = typeof(TKafkaConfiguration).Name;

            var kafkaConfiguration = builder.Configuration
                .GetSection(kafkaSectionName)
                .Get<TKafkaConfiguration>();

            if (kafkaConfiguration is null || !kafkaConfiguration.IsValid)
            {
                throw new ArgumentException($"'{kafkaSectionName}' is not configured!");
            }

            var consumerSectionName = typeof(TConsumerConfiguration).Name;

            builder.Services.Configure<TConsumerConfiguration>(builder.Configuration
                .GetSection(consumerSectionName));

            var consumerConfiguration = builder.Configuration
                .GetSection(consumerSectionName)
                .Get<TConsumerConfiguration>();

            if (consumerConfiguration is null || !consumerConfiguration.IsValid)
            {
                throw new ArgumentException($"'{consumerSectionName}' is not configured!");
            }

            builder.Services.AddSingleton<IMessageConsumerService<TKafkaConfiguration, TConsumerConfiguration>, KafkaConsumerService<TKafkaConfiguration, TConsumerConfiguration>>();
            builder.Services.AddHostedService<KafkaConsumerBackgroundService<TKafkaConfiguration, TConsumerConfiguration, TConsumerMessage>>();

            return builder;
        }

        public static WebApplicationBuilder AddKafkaHealthCheck<TKafkaConfiguration, IConfigTopic>(this WebApplicationBuilder builder)
            where TKafkaConfiguration : KafkaConfigurationBase
            where IConfigTopic : class, ITopicConfiguration
        {
            var kafkaSectionName = typeof(TKafkaConfiguration).Name;

            var kafkaConfiguration = builder.Configuration
                .GetSection(kafkaSectionName)
                .Get<TKafkaConfiguration>();

            if (kafkaConfiguration is null || !kafkaConfiguration.IsValid)
            {
                throw new ArgumentException($"'{kafkaSectionName}' is not configured!");
            }

            builder.Services.AddScoped<IKafkaInfoService<TKafkaConfiguration, IConfigTopic>, KafkaInfoService<TKafkaConfiguration, IConfigTopic>>();

            return builder;
        }
    }
}
