using Application.DTO.ActivityLog;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Контроллер для работы с логами активности
/// </summary>
/// <remarks>
/// Предоставляет API для создания, получения и удаления записей логов активности
/// </remarks>
[ApiController, ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/activity-log")]
public class ActivityLogController : BaseController<IActivityLogService>
{
    /// <summary>
    /// Инициализирует новый экземпляр контроллера ActivityLogController
    /// </summary>
    /// <param name="activityLogsService">Сервис для работы с логами активности</param>
    public ActivityLogController(IActivityLogService activityLogsService) : base(activityLogsService)
    { }

    /// <summary>
    /// Создает новую запись в логе активности
    /// </summary>
    /// <param name="request">Данные для создания записи</param>
    /// <returns>Результат операции с ID созданной записи</returns>
    /// <response code="201">Запись успешно создана</response>
    /// <response code="400">Ошибка валидации данных</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPost]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(CreateActivityLogRequest request)
    {
        var result = await _service.CreateAsync(request, MethodType.Create);
        return GetResponse(result);
    }

    /// <summary>
    /// Получает список записей лога активности с фильтрацией
    /// </summary>
    /// <param name="request">Параметры фильтрации</param>
    /// <returns>Список записей лога активности</returns>
    /// <response code="200">Успешное получение данных</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpGet]
    [ProducesResponseType(typeof(ActivityLogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromQuery] FilteringActivityLogRequest request)
    {
        var result = await _service.GetAsync(request);
        return GetResponse(result);
    }

    /// <summary>
    /// Удаляет запись из лога активности по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор записи</param>
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