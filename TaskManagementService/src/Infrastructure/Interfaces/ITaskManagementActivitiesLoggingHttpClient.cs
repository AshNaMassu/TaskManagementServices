using Infrastructure.DTO;
using Refit;

namespace Infrastructure.Interfaces
{
    public interface ITaskManagementActivitiesLoggingHttpClient
    {
        [Post("/api/v1/activity-log")]
        Task<IApiResponse> CreateAsync(CreateActivityLogRequest request);
    }
}
