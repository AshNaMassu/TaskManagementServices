using Application.DTO.Logging;
using Application.Interfaces;
using AutoMapper;
using Infrastructure.DTO;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class ActivityLogHttpSenderService : IActivityLogSenderService
    {
        private readonly ITaskManagementActivitiesLoggingHttpClient _taskManagementActivitiesLoggingHttpClient;
        private readonly ILogger<ActivityLogKafkaProducerService> _logger;
        private readonly IMapper _mapper;

        public ActivityLogHttpSenderService(ITaskManagementActivitiesLoggingHttpClient taskManagementActivitiesLoggingHttpClient, ILogger<ActivityLogKafkaProducerService> logger, IMapper mapper)
        {
            _taskManagementActivitiesLoggingHttpClient = taskManagementActivitiesLoggingHttpClient;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task SendMessage(EntityChangedMessage message)
        {
            var request = _mapper.Map<CreateActivityLogRequest>(message);

            try
            {
                var response = await _taskManagementActivitiesLoggingHttpClient.CreateAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    string messageError = $"Error sending request. Service: {nameof(ActivityLogHttpSenderService)}. StatusCode: {response.StatusCode}. Error: {response.Error}";
                    _logger.LogError(messageError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{nameof(ActivityLogHttpSenderService)}]: HttpSender error: {ex.Message}");
            }
        }
    }
}
