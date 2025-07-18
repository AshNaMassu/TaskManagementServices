using Application.DTO.HealthCheck;
using Application.Enums;
using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Persistence.Database.Context;

namespace Persistence.Repositories
{
    public class HealthCheckRepository : IHealthCheckRepository
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<HealthCheckRepository> _logger;

        public HealthCheckRepository(IDbContextFactory dbContext, ILogger<HealthCheckRepository> logger)
        {
            _dbContextFactory = dbContext;
            _logger = logger;
        }

        public async Task<HealthCheck> CheckHealthDbAsync()
        {
            try
            {
                await using var dbContext = _dbContextFactory.CreateDbContext();

                var isCanConnected = await dbContext.Database.CanConnectAsync();

                if (isCanConnected)
                {
                    return new HealthCheck { Status = HealthCheckStatus.Ok,};
                }
                else
                {
                    return new HealthCheck { Status = HealthCheckStatus.Failed, Message = "Database connection test failed" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new HealthCheck { Status = HealthCheckStatus.Failed, Message = ex.Message };
            }
        }
    }
}
