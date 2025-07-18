using Application.DTO.Task;

namespace Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        public Task<List<TaskResponse>> GetAsync(FilteringTaskRequest request);
        public Task<TaskResponse> GetAsync(long id);
        public Task<long> CreateAsync(CreateTaskRequest createTaskRequest);
        public Task UpdateAsync(long id, UpdateTaskRequest updateTaskRequest);
        public Task UpdateStatusAsync(long id, UpdateTaskStatusRequest updateTaskRequest);
        public Task DeleteAsync(long id);
        public Task<bool> ExistsById(long id);
    }
}
