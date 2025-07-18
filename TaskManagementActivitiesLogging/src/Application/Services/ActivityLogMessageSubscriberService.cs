using Application.DTO.ActivityLog;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class ActivityLogMessageSubscriberService : IEventBusSubscriber<ActivityLogConsumerMessage>
    {
        private readonly ILogger<ActivityLogMessageSubscriberService> _logger;
        private readonly IActivityLogHandler _activityLogHandler;

        public ActivityLogMessageSubscriberService(ILogger<ActivityLogMessageSubscriberService> logger, IActivityLogHandler activityLogHandler)
        {
            _logger = logger;
            _activityLogHandler = activityLogHandler;
        }

        public async Task<bool> Handle(ActivityLogConsumerMessage message)
        {
            if (message is null)
            {
                _logger.LogError("Incorrect data. Input message is null");
                return false;
            }

            var result = await _activityLogHandler.HandleAsync(message);

            return true;
        }
    }
}
