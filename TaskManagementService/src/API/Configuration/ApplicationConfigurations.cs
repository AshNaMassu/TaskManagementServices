using Application.Interfaces;
using Application.Services;

namespace API.Configuration
{
    public static class ApplicationConfigurations
    {
        public static WebApplicationBuilder AddApplication(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IHealthCheckService, HealthCheckService>();
            builder.Services.AddTransient<IChangesLoggingService, ChangesLoggingService>();
            builder.Services.AddTransient<ITaskService, TaskService>();

            return builder;
        }
    }
}
