using Application.DTO.ActivityLog;

namespace Application.Interfaces
{
    public interface ILoggingService
    {
        void Log<T>(T message) where T : CreateActivityLog;
    }
}
