using Application.DTO.ActivityLog;
using Application.DTO.Result;

namespace Application.Interfaces
{
    public interface IActivityLogService
    {
        public Task<MethodResult> CreateAsync(CreateActivityLogRequest request);
        public Task<MethodResult> DeleteAsync(long id);
        public Task<MethodResult<List<ActivityLogResponse>>> GetAsync(FilteringActivityLogRequest request);
    }
}
