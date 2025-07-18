using Application.DTO.ActivityLog;

namespace Application.Interfaces.Repositories
{
    public interface IActivityLogRepository
    {
        Task CreateAsync<T>(T request) where T : CreateActivityLog;
        Task DeleteAsync(long id);
        Task<List<ActivityLogResponse>> GetAsync(FilteringActivityLogRequest request);
        Task<bool> ExistsById(long id);
    }
}
