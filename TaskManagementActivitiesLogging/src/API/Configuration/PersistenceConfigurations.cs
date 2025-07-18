using API.Profiles;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using Persistence.Database.Context;
using Persistence.Repositories;

namespace API.Configuration
{
    public static class PersistenceConfigurations
    {
        public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<DataBaseContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAutoMapper(typeof(MapProfile));
            builder.Services.AddSingleton<IDbContextFactory, DbContextFactory>();
            builder.Services.AddTransient<IHealthCheckRepository, HealthCheckRepository>();
            builder.Services.AddTransient<IActivityLogRepository, ActivityLogRepository>();

            return builder;
        }
    }
}
