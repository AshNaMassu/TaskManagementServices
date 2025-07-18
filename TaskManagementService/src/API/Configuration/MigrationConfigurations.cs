using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace API.Configuration
{
    public static class MigrationConfigurations
    {
        public static IApplicationBuilder ApplyMigration(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
                dbContext.Database.Migrate();
            }

            return app;
        }

    }
}
