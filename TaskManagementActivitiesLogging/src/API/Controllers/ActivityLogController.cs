using Application.DTO.ActivityLog;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController, ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/activity-log")]
public class ActivityLogController : BaseController<IActivityLogService>
{
    public ActivityLogController(IActivityLogService activityLogsService) : base(activityLogsService)
    { }

    [HttpPost]
    public async Task<IActionResult> Create(CreateActivityLogRequest request)
    {
        var result = await _service.CreateAsync(request);
        return GetResponse(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] FilteringActivityLogRequest request)
    {
        var result = await _service.GetAsync(request);
        return GetResponse(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _service.DeleteAsync(id);
        return GetResponse(result);
    }
}