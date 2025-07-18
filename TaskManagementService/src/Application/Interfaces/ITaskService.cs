using Application.DTO.Result;
using Application.DTO.Task;

namespace Application.Interfaces
{
    public interface ITaskService
    {
        public Task<MethodResult<List<TaskResponse>>> GetAsync(FilteringTaskRequest request);
        public Task<MethodResult<TaskResponse>> GetAsync(long id);
        public Task<MethodResult<long>> CreateAsync(CreateTaskRequest createTaskRequest);
        public Task<MethodResult> UpdateAsync(long id, UpdateTaskRequest updateTaskRequest);
        public Task<MethodResult> UpdateStatusAsync(long id, UpdateTaskStatusRequest updateTaskStatusRequest);
        public Task<MethodResult> DeleteAsync(long id);
    }
}
