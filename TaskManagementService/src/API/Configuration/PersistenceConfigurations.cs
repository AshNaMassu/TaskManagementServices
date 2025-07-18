using API.Profiles;
using Application.Interfaces.Repositories;
using Persistence.Database.Context;
using Persistence.Repositories;

namespace API.Configuration
{
    public static class PersistenceConfigurations
    {
        public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(MapProfile));
            builder.Services.AddSingleton<IDbContextFactory, DbContextFactory>();
            builder.Services.AddTransient<IHealthCheckRepository, HealthCheckRepository>();
            builder.Services.AddTransient<ITaskRepository, TaskRepository>();

            return builder;
        }
    }
}
