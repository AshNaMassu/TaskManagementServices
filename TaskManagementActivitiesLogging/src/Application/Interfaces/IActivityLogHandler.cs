using Application.DTO.ActivityLog;
using Application.DTO.Result;

namespace Application.Interfaces
{
    public interface IActivityLogHandler
    {
        Task<MethodResult> HandleAsync<T>(T activityLog) where T : CreateActivityLog;
    }
}
