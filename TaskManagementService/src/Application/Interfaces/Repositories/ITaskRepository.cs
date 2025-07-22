using Application.DTO.Task;

namespace Application.Interfaces.Repositories
{
    /// <summary>
    /// Репозиторий для работы с задачами
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// Получает список задач с фильтрацией
        /// </summary>
        public Task<List<TaskResponse>> GetAsync(FilteringTaskRequest request);

        /// <summary>
        /// Получает задачу по ID
        /// </summary>
        public Task<TaskResponse> GetAsync(long id);

        /// <summary>
        /// Создает новую задачу
        /// </summary>
        /// <returns>ID созданной задачи</returns>
        public Task<long> CreateAsync(CreateTaskRequest createTaskRequest);

        /// <summary>
        /// Обновляет задачу
        /// </summary>
        public Task UpdateAsync(long id, UpdateTaskRequest updateTaskRequest);

        /// <summary>
        /// Обновляет статус задачи
        /// </summary>
        public Task UpdateStatusAsync(long id, UpdateTaskStatusRequest updateTaskRequest);

        /// <summary>
        /// Удаляет задачу
        /// </summary>
        public Task DeleteAsync(long id);

        /// <summary>
        /// Проверяет существование задачи по ID
        /// </summary>
        public Task<bool> ExistsById(long id);
    }
}
