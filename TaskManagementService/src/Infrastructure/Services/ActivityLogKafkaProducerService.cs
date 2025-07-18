using Application.DTO.Logging;
using Application.Interfaces;
using AutoMapper;
using Infrastructure.Confuguration;
using Infrastructure.DTO;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class ActivityLogKafkaProducerService : IActivityLogSenderService
    {
        private readonly IEventBusPublisher<ActivityLogProducerOptions> _eventBusPublisher;
        private readonly ILogger<ActivityLogKafkaProducerService> _logger;
        private readonly IMapper _mapper;

        public ActivityLogKafkaProducerService(IEventBusPublisher<ActivityLogProducerOptions> eventBusPublisher, ILogger<ActivityLogKafkaProducerService> logger, IMapper mapper)
        {
            _eventBusPublisher = eventBusPublisher;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task SendMessage(EntityChangedMessage message)
        {
            var activityLogMessage = _mapper.Map<ActivityLogMessage>(message);

            try
            {
                await _eventBusPublisher.PublishAsync(activityLogMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{nameof(ActivityLogKafkaProducerService)}]: Kafka error: {ex.Message}");
            }
        }
    }
}
