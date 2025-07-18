using Persistence.Configurations;

namespace API.Configuration
{
    public static class OptionsConfigutations
    {
        public static WebApplicationBuilder ConfigureOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<PostgresDatabaseOptions>(
                builder.Configuration.GetSection(nameof(PostgresDatabaseOptions)));

            return builder;
        }
    }
}
