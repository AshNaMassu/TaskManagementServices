using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTO.HealthCheck;
using Application.Enums;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер для проверки состояния системы
    /// </summary>
    [Route("info")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly IHealthCheckService _healthCheckService;

        public InfoController(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        /// <summary>
        /// Проверяет состояние сервиса
        /// </summary>
        /// <returns>Результат проверки состояния сервиса</returns>
        /// <response code="200">Сервис работает нормально</response>
        /// <response code="503">Проблемы в работе сервиса</response>
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
