using Application.DTO.Logging;
using Application.DTO.Result;
using Application.DTO.Task;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskService> _logger;
        private readonly IChangesLoggingService _changesLoggingService;

        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger, IChangesLoggingService changesLoggingService)
        {
            _taskRepository = taskRepository;
            _logger = logger;
            _changesLoggingService = changesLoggingService;
        }

        
        public async Task<MethodResult<long>> CreateAsync(CreateTaskRequest createTaskRequest)
        {
            try
            {
                var id = await _taskRepository.CreateAsync(createTaskRequest);

                var entityChangedMessage = new EntityChangedMessage
                {
                    Operation = nameof(CreateAsync),
                    Entity = nameof(TaskEntity),
                    EntityId = id
                };

                await _changesLoggingService.Log(entityChangedMessage);

                return MethodResult<long>.Success(id);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to create task: {ex.Message}";
                LogError(errorMessage, nameof(CreateAsync));

                return MethodResult<long>.InternalError(errorMessage);
            }
        }


        public async Task<MethodResult> DeleteAsync(long id)
        {
            if (!await _taskRepository.ExistsById(id))
            {
                var errorMessage = $"{nameof(TaskEntity)} with id {id} not found";
                LogError(errorMessage, nameof(DeleteAsync));
                return MethodResult.NotFound(errorMessage);
            }

            try
            {
                await _taskRepository.DeleteAsync(id);

                var entityChangedMessage = new EntityChangedMessage
                {
                    Operation = nameof(DeleteAsync),
                    Entity = nameof(TaskEntity),
                    EntityId = id
                };

                await _changesLoggingService.Log(entityChangedMessage);

                return MethodResult.Success();
            }
            catch (NotFoundException ex)
            {
                var errorMessage = $"Failed to delete task: {ex.Message}";
                LogError(errorMessage, nameof(DeleteAsync));
                return MethodResult.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to delete task: {ex.Message}";
                LogError(errorMessage, nameof(DeleteAsync));

                return MethodResult.InternalError(errorMessage);
            }
        }


        public async Task<MethodResult<List<TaskResponse>>> GetAsync(FilteringTaskRequest request)
        {
            try
            {
                var tasks = await _taskRepository.GetAsync(request);
                return MethodResult<List<TaskResponse>>.Success(tasks);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to get task: {ex.Message}";
                LogError(errorMessage, nameof(GetAsync));

                return MethodResult<List<TaskResponse>>.InternalError(errorMessage);
            }
        }


        public async Task<MethodResult<TaskResponse>> GetAsync(long id)
        {
            if (!await _taskRepository.ExistsById(id))
            {
                var errorMessage = $"{nameof(TaskEntity)} with id {id} not found";
                LogError(errorMessage, nameof(GetAsync));
                return MethodResult<TaskResponse>.NotFound(errorMessage);
            }

            try
            {
                var task = await _taskRepository.GetAsync(id);
                return MethodResult<TaskResponse>.Success(task);
            }
            catch (NotFoundException ex)
            {
                var errorMessage = $"Failed to get task: {ex.Message}";
                LogError(errorMessage, nameof(GetAsync));

                return MethodResult<TaskResponse>.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to get task: {ex.Message}";
                LogError(errorMessage, nameof(GetAsync));

                return MethodResult<TaskResponse>.InternalError(errorMessage);
            }
        }


        public async Task<MethodResult> UpdateAsync(long id, UpdateTaskRequest updateTaskRequest)
        {
            if (!await _taskRepository.ExistsById(id))
            {
                var errorMessage = $"{nameof(TaskEntity)} with id {id} not found";
                LogError(errorMessage, nameof(UpdateAsync));
                return MethodResult.NotFound(errorMessage);
            }

            try
            {
                await _taskRepository.UpdateAsync(id, updateTaskRequest);

                var entityChangedMessage = new EntityChangedMessage
                {
                    Operation = nameof(UpdateAsync),
                    Entity = nameof(TaskEntity),
                    EntityId = id
                };

                await _changesLoggingService.Log(entityChangedMessage);

                return MethodResult.Success();
            }
            catch (NotFoundException ex)
            {
                var errorMessage = $"Failed to update task: {ex.Message}";
                LogError(errorMessage, nameof(UpdateAsync));

                return MethodResult.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to update task: {ex.Message}";
                LogError(errorMessage, nameof(UpdateAsync));

                return MethodResult.InternalError(errorMessage);
            }
        }


        public async Task<MethodResult> UpdateStatusAsync(long id, UpdateTaskStatusRequest updateTaskStatusRequest)
        {
            if (!await _taskRepository.ExistsById(id))
            {
                var errorMessage = $"{nameof(TaskEntity)} with id {id} not found";
                LogError(errorMessage, nameof(UpdateStatusAsync));
                return MethodResult.NotFound(errorMessage);
            }

            try
            {
                await _taskRepository.UpdateStatusAsync(id, updateTaskStatusRequest);

                var entityChangedMessage = new EntityChangedMessage
                {
                    Operation = nameof(UpdateStatusAsync),
                    Entity = nameof(TaskEntity),
                    EntityId = id
                };

                await _changesLoggingService.Log(entityChangedMessage);

                return MethodResult.Success();
            }
            catch (NotFoundException ex)
            {
                var errorMessage = $"Failed to update task status: {ex.Message}";
                LogError(errorMessage, nameof(UpdateStatusAsync));
                return MethodResult.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to update task status: {ex.Message}";
                LogError(errorMessage, nameof(UpdateStatusAsync));

                return MethodResult.InternalError(errorMessage);
            }
        }

        private void LogError(string message, string method)
        {
            _logger.LogError($"[{nameof(TaskService)}][{method}]: {message}");
        }
    }
}
