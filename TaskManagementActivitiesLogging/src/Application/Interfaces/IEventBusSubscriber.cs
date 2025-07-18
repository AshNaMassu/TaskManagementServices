namespace Application.Interfaces
{
    public interface IEventBusSubscriber<TMessage> where TMessage : class
    {
        Task<bool> Handle(TMessage message);
    }
}
