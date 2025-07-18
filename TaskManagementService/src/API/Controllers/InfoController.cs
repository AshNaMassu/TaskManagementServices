using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTO.HealthCheck;
using Application.Enums;

namespace API.Controllers
{
    [Route("info")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly IHealthCheckService _healthCheckService;

        public InfoController(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet("healthcheck")]
        public async Task<IActionResult> HealthCheck()
        {
            var healthCheck = await _healthCheckService.CheckHealthAsync();

            var response = new HealthCheckResponse
            {
                Status = healthCheck.Status.ToString(),
                Message = healthCheck.Message
            };

            return StatusCode(healthCheck.Status == HealthCheckStatus.Ok ? StatusCodes.Status200OK : StatusCodes.Status503ServiceUnavailable, response);
        }
    }
}
