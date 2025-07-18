using Application.DTO.Task;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController, ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/task")]
    public class TaskController : BaseController<ITaskService>
    {
        public TaskController(ITaskService service) : base(service)
        { }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] FilteringTaskRequest model)
        {
            var result = await _service.GetAsync(model);
            return GetResponse(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetAsync(id);
            return GetResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskRequest request)
        {
            var result = await _service.CreateAsync(request);
            return GetResponse(result);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, UpdateTaskRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            return GetResponse(result);
        }

        [HttpPatch("{id:long}/status")]
        public async Task<IActionResult> UpdateStatus(long id, UpdateTaskStatusRequest request)
        {
            var result = await _service.UpdateStatusAsync(id, request);
            return GetResponse(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteAsync(id);
            return GetResponse(result);
        }
    }
}
