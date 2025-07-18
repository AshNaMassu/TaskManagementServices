using Application.DTO.Logging;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class ChangesLoggingService : IChangesLoggingService
    {
        private readonly ILogger<ChangesLoggingService> _logger;
        private readonly IActivityLogSenderService _activityLogProducerService;

        public ChangesLoggingService(ILogger<ChangesLoggingService> logger, IActivityLogSenderService activityLogProducerService)
        {
            _logger = logger;
            _activityLogProducerService = activityLogProducerService;
        }

        public async Task Log(EntityChangedMessage message)
        {
            await _activityLogProducerService.SendMessage(message);
            _logger.LogInformation($"An action to change DB data occurred. Entity = {message.Entity}, EntityId = {message.EntityId}, Operation = {message.Operation}, DateTime = {DateTime.UtcNow}");
        }
    }
}
