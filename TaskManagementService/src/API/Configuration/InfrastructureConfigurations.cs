using Application.Interfaces;
using Infrastructure.Confuguration;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Refit;

namespace API.Configuration
{
    public static class InfrastructureConfigurations
    {
        public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
        {
            builder.AddKafka<KafkaConfigurationOptions>();
            builder.AddKafkaProducer<KafkaConfigurationOptions, ActivityLogProducerOptions>();
            builder.AddKafkaHealthCheck<KafkaConfigurationOptions, ActivityLogProducerOptions>();


            builder.Services.AddTransient<IActivityLogSenderService, ActivityLogKafkaProducerService>();
            builder.Services.AddTransient<IActivityLogSenderService, ActivityLogHttpSenderService>();
            builder.Services.AddTransient<IInfrastructureInfoService, InfrastructureInfoService>();

            var refitSettings = new RefitSettings
            {
                CollectionFormat = CollectionFormat.Multi
            };

            builder.Services.AddRefitClient<ITaskManagementActivitiesLoggingHttpClient>(refitSettings)
            .ConfigureHttpClient(client => client.BaseAddress = new Uri(
                    builder.Configuration.GetValue<string>(nameof(ITaskManagementActivitiesLoggingHttpClient))));


            var logSender = builder.Configuration.GetValue<string>("LogSender");

            if (logSender?.ToLower() == "kafka")
            {
                builder.Services.AddTransient<IActivityLogSenderService, ActivityLogKafkaProducerService>();
            }
            else
            {
                builder.Services.AddTransient<IActivityLogSenderService, ActivityLogHttpSenderService>();
            }

            return builder;
        }
    }
}
