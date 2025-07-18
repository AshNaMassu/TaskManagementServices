using Application.DTO.ActivityLog;
using Application.DTO.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;

namespace Application.Services
{
    public class ActivityLogHandler : IActivityLogHandler
    {
        private readonly IActivityLogRepository _repository;
        private readonly ILoggingService _loggingService;

        public ActivityLogHandler(IActivityLogRepository repository, ILoggingService loggingService)
        {
            _repository = repository;
            _loggingService = loggingService;
        }

        public async Task<MethodResult> HandleAsync<T>(T activityLog) where T : CreateActivityLog
        {
            _loggingService.Log(activityLog);
            try
            {
                await _repository.CreateAsync(activityLog);
                return MethodResult.Success();
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to create activity log: {ex.Message}";

                return MethodResult.InternalError(errorMessage);
            }
        }
    }
}
