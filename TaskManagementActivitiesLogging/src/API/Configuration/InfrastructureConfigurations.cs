using Application.DTO.ActivityLog;
using Application.Interfaces;
using Infrastructure.Confuguration;
using Infrastructure.Extensions;
using Infrastructure.Services;

namespace API.Configuration
{
    public static class InfrastructureConfigurations
    {
        public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
        {
            builder.AddKafka<KafkaConfigurationOptions>();
            builder.AddKafkaConsumer<KafkaConfigurationOptions, ActivityLogConsumerOptions, ActivityLogConsumerMessage>();
            builder.AddKafkaHealthCheck<KafkaConfigurationOptions, ActivityLogConsumerOptions>();

            builder.Services.AddTransient<IInfrastructureInfoService, InfrastructureInfoService>();

            return builder;
        }
    }
}
