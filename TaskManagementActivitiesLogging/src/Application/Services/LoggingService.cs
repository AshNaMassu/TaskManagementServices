using Application.DTO.ActivityLog;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public void Log<T>(T message) where T : CreateActivityLog
        {
            _logger.LogInformation($"[ActivityLog] EventType: {message.EventType}, Entity: {message.Entity}, EntityId: {message.EntityId}, EventTime: {message.EventTime:yyyy-MM-dd HH:mm:ss}");
        }
    }
}
