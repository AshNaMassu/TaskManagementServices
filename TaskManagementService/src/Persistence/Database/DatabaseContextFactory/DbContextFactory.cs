using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using Persistence.Configurations;

namespace Persistence.Database.Context
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly string _postgresConnectionString;

        public DbContextFactory(IOptions<PostgresDatabaseOptions> postgresOptions)
        {
            var postgreDatabaseOptions = postgresOptions.Value;
            if (!string.IsNullOrEmpty(postgreDatabaseOptions.Host))
            {
                _postgresConnectionString = BuildPostgreSqlConnectionString(postgreDatabaseOptions);
            }
        }

        public DataBaseContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataBaseContext>();
            optionsBuilder.UseNpgsql(_postgresConnectionString);

            return new DataBaseContext(optionsBuilder.Options);
        }

        private string BuildPostgreSqlConnectionString(PostgresDatabaseOptions options)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder()
            {
                Host = options.Host,
                Port = options.Port,
                SearchPath = options.Schema,
                Database = options.Name,
                Username = options.User,
                Password = options.Password,
                CommandTimeout = options.CommandTimeout,
                ConnectionIdleLifetime = options.ConnectionIdLifetime,
                Pooling = options.Pooling,
                KeepAlive = options.KeepAlive,
                TcpKeepAlive = options.TcpKeepAlive
            };

            return connectionStringBuilder.ConnectionString;
        }
    }
}
