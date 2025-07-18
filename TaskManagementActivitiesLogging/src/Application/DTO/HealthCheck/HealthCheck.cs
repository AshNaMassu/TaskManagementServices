using Application.Enums;

namespace Application.DTO.HealthCheck
{
    public class HealthCheck
    {
        public HealthCheckStatus Status { get; set; }

        public string Message { get; set; }
    }
}
