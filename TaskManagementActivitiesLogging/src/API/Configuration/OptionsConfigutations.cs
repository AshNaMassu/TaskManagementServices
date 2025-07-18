using Infrastructure.Confuguration;
using Persistence.Configurations;

namespace API.Configuration
{
    public static class OptionsConfigutations
    {
        public static WebApplicationBuilder ConfigureOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<PostgresDatabaseOptions>(
                builder.Configuration.GetSection(nameof(PostgresDatabaseOptions)));

            builder.Services.Configure<KafkaConfigurationOptions>(
                builder.Configuration.GetSection(nameof(KafkaConfigurationOptions)));

            builder.Services.Configure<ActivityLogConsumerOptions>(
                builder.Configuration.GetSection(nameof(ActivityLogConsumerOptions)));

            return builder;
        }
    }
}
