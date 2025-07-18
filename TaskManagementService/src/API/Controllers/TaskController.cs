using Application.DTO.Task;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер для управления задачами
    /// </summary>
    /// <remarks>
    /// Предоставляет CRUD-операции и управление статусами задач
    /// </remarks>
    [ApiController, ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/task")]
    public class TaskController : BaseController<ITaskService>
    {
        // <summary>
        /// Инициализирует новый экземпляр контроллера задач
        /// </summary>
        /// <param name="service">Сервис для работы с задачами</param>
        public TaskController(ITaskService service) : base(service)
        { }

        /// <summary>
        /// Получает список задач с возможностью фильтрации
        /// </summary>
        /// <param name="model">Параметры фильтрации и пагинации</param>
        /// <returns>Список задач, соответствующих критериям</returns>
        /// <response code="200">Успешно возвращен список задач</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<TaskResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] FilteringTaskRequest model)
        {
            var result = await _service.GetAsync(model);
            return GetResponse(result);
        }

        /// <summary>
        /// Получает задачу по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>Список задач, соответствующих критериям</returns>
        /// <response code="200">Успешно возвращен список задач</response>
        /// <response code="404">Запись не найдена</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetAsync(id);
            return GetResponse(result);
        }

        /// <summary>
        /// Создает новую задачу
        /// </summary>
        /// <param name="request">Данные для создания задачи</param>
        /// <returns>Статус созданной задачи</returns>
        /// <response code="201">Задача успешно создана</response>
        /// <response code="400">Некорректные данные задачи</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(CreateTaskRequest request)
        {
            var result = await _service.CreateAsync(request);
            return GetResponse(result, MethodType.Create);
        }

        /// <summary>
        /// Обновляет существующую задачу
        /// </summary>
        /// <param name="id">Идентификатор обновляемой задачи</param>
        /// <param name="request">Новые данные задачи</param>
        /// <returns>Результат операции изменения</returns>
        /// <response code="200">Задача успешно обновлена</response>
        /// <response code="400">Некорректные данные задачи</response>
        /// <response code="404">Задача не найдена</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPut("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(long id, UpdateTaskRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            return GetResponse(result);
        }

        /// <summary>
        /// Обновляет статус задачи
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="request">Новый статус задачи</param>
        /// <returns>Результат операции изменения</returns>
        /// <response code="200">Статус успешно обновлен</response>
        /// <response code="400">Некорректный статус</response>
        /// <response code="404">Задача не найдена</response>
        /// <response code="409">Конфликт при обновлении статуса</response>
        [HttpPatch("{id:long}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStatus(long id, UpdateTaskStatusRequest request)
        {
            var result = await _service.UpdateStatusAsync(id, request);
            return GetResponse(result);
        }

        /// <summary>
        /// Удаляет задачу
        /// </summary>
        /// <param name="id">Идентификатор удаляемой задачи</param>
        /// <returns>Результат операции удаления</returns>
        /// <response code="204">Запись успешно удалена</response>
        /// <response code="404">Запись не найдена</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteAsync(id);
            return GetResponse(result, MethodType.Delete);
        }
    }
}
