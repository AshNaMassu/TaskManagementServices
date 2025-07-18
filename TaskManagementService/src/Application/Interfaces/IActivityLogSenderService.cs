using Application.DTO.Logging;

namespace Application.Interfaces
{
    public interface IActivityLogSenderService
    {
        public Task SendMessage(EntityChangedMessage message);
    }
}
