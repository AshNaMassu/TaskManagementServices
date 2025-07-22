using Application.DTO.Result;
using Application.DTO.Task;

namespace Application.Interfaces
{
    /// <summary>
    /// Сервис для работы с задачами
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Получает список задач с возможностью фильтрации
        /// </summary>
        /// <param name="request">Параметры фильтрации</param>
        /// <returns>Результат операции со списком задач</returns>
        public Task<MethodResult<List<TaskResponse>>> GetAsync(FilteringTaskRequest request);

        /// <summary>
        /// Получает задачу по ID
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>Результат операции с данными задачи</returns>
        public Task<MethodResult<TaskResponse>> GetAsync(long id);

        /// <summary>
        /// Создает новую задачу
        /// </summary>
        /// <param name="createTaskRequest">Данные для создания задачи</param>
        /// <returns>Результат операции с ID созданной задачи</returns>
        public Task<MethodResult<long>> CreateAsync(CreateTaskRequest createTaskRequest);

        /// <summary>
        /// Обновляет задачу
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="updateTaskRequest">Новые данные задачи</param>
        /// <returns>Результат операции</returns>
        public Task<MethodResult> UpdateAsync(long id, UpdateTaskRequest updateTaskRequest);

        /// <summary>
        /// Обновляет статус задачи
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="updateTaskStatusRequest">Новый статус</param>
        /// <returns>Результат операции</returns>
        public Task<MethodResult> UpdateStatusAsync(long id, UpdateTaskStatusRequest updateTaskStatusRequest);

        /// <summary>
        /// Удаляет задачу
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>Результат операции</returns>
        public Task<MethodResult> DeleteAsync(long id);
    }
}
