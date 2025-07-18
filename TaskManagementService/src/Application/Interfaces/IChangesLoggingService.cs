using Application.DTO.Logging;

namespace Application.Interfaces
{
    public interface IChangesLoggingService
    {
        Task Log(EntityChangedMessage message);
    }
}
