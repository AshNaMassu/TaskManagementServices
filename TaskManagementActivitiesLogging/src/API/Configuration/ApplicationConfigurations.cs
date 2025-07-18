using Application.DTO.ActivityLog;
using Application.Interfaces;
using Application.Services;

namespace API.Configuration
{
    public static class ApplicationConfigurations
    {
        public static WebApplicationBuilder AddApplication(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IHealthCheckService, HealthCheckService>();
            builder.Services.AddTransient<IActivityLogService, ActivityLogService>();
            builder.Services.AddTransient<IEventBusSubscriber<ActivityLogConsumerMessage>, ActivityLogMessageSubscriberService>();
            builder.Services.AddTransient<ILoggingService, LoggingService>();
            builder.Services.AddTransient<IActivityLogHandler, ActivityLogHandler>();

            return builder;
        }
    }
}
