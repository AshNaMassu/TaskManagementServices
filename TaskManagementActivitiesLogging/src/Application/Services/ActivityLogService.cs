using Application.DTO.ActivityLog;
using Application.DTO.Result;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly ILogger<ActivityLogService> _logger;
        private readonly IActivityLogHandler _activityLogHandler;

        public ActivityLogService(IActivityLogRepository activityLogRepository, ILogger<ActivityLogService> logger, IActivityLogHandler activityLogHandler)
        {
            _activityLogRepository = activityLogRepository;
            _logger = logger;
            _activityLogHandler = activityLogHandler;
        }

        public async Task<MethodResult> CreateAsync(CreateActivityLogRequest request)
        {
            return await _activityLogHandler.HandleAsync(request);
        }

        public async Task<MethodResult> DeleteAsync(long id)
        {
            if (!await _activityLogRepository.ExistsById(id))
            {
                var errorMessage = $"{nameof(ActivityLog)} with id {id} not found";
                LogError(errorMessage, nameof(DeleteAsync));
                return MethodResult.NotFound(errorMessage);
            }

            try
            {
                await _activityLogRepository.DeleteAsync(id);
                return MethodResult.Success();
            }
            catch (NotFoundException ex)
            {
                return MethodResult.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to delete activity log: {ex.Message}";
                LogError(errorMessage, nameof(DeleteAsync));

                return MethodResult.InternalError(errorMessage);
            }
        }

        public async Task<MethodResult<List<ActivityLogResponse>>> GetAsync(FilteringActivityLogRequest request)
        {
            try
            {
                var activityLogs = await _activityLogRepository.GetAsync(request);
                return MethodResult<List<ActivityLogResponse>>.Success(activityLogs);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to get activity logs: {ex.Message}";
                LogError(errorMessage, nameof(GetAsync));

                return MethodResult<List<ActivityLogResponse>>.InternalError(errorMessage);
            }
        }

        private void LogError(string message, string method)
        {
            _logger.LogError($"[{nameof(ActivityLogService)}][{method}]: {message}");
        }
    }
}
