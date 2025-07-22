using Application.Enums;

namespace Application.DTO.HealthCheck
{
    /// <summary>
    /// Состояния сервиса
    /// </summary>
    public class HealthCheck
    {
        /// <summary>
        /// Текущий статус сервиса
        /// </summary>
        public HealthCheckStatus Status { get; set; }

        /// <summary>
        /// Дополнительное сообщение о состоянии
        /// </summary>
        public string Message { get; set; }
    }
}
