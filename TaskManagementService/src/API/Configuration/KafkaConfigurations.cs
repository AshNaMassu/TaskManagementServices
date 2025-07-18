using Application.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Interfaces;
using Infrastructure.Services;

namespace API.Configuration
{
    public static class KafkaConfigurations
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

        public static WebApplicationBuilder AddKafkaProducer<TKafkaConfiguration, TProducerConfiguration>(this WebApplicationBuilder builder)
            where TKafkaConfiguration : KafkaConfigurationBase
            where TProducerConfiguration : ProducerConfigurationBase
        {
            var kafkaSectionName = typeof(TKafkaConfiguration).Name;

            var kafkaConfiguration = builder.Configuration
                .GetSection(kafkaSectionName)
                .Get<TKafkaConfiguration>();

            if (kafkaConfiguration is null || !kafkaConfiguration.IsValid)
            {
                throw new ArgumentException($"'{kafkaSectionName}' is not configured!");
            }

            var producerSectionName = typeof(TProducerConfiguration).Name;

            builder.Services.Configure<TProducerConfiguration>(builder.Configuration
                .GetSection(producerSectionName));

            var producerConfiguration = builder.Configuration
                .GetSection(producerSectionName)
                .Get<TProducerConfiguration>();

            if (producerConfiguration is null || !producerConfiguration.IsValid)
            {
                throw new ArgumentException($"'{producerSectionName}' is not configured!");
            }

            builder.Services.AddSingleton<IEventBusPublisher<TProducerConfiguration>, KafkaProducerService<TKafkaConfiguration, TProducerConfiguration>>();

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
