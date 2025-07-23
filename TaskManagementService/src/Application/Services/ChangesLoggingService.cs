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
            var (isValid, errorMessage) = ValidateEntityChangedMessage(message);
            if (!isValid)
            {
                _logger.LogWarning("Validation failed: {ErrorMessage}", errorMessage);
                throw new ArgumentException(errorMessage);
            }

            await _activityLogProducerService.SendMessage(message);

            _logger.LogInformation($"An action to change DB data occurred. Entity = {message.Entity}, EntityId = {message.EntityId}, Operation = {message.Operation}, DateTime = {DateTime.UtcNow}");
        }

        private (bool isValid, string errorMessage) ValidateEntityChangedMessage(EntityChangedMessage message)
        {
            if (message == null)
                return (false, "Message cannot be null");

            if (string.IsNullOrWhiteSpace(message.Operation))
                return (false, "Operation is required");

            if (string.IsNullOrWhiteSpace(message.Entity))
                return (false, "Entity name is required");

            if (message.EntityId <= 0)
                return (false, "Entity ID must be positive");

            return (true, null);
        }
    }
}
