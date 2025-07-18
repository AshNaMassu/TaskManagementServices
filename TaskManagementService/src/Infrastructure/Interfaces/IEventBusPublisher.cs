namespace Application.Interfaces
{
    public interface IEventBusPublisher<TSettings>
    {
        Task PublishAsync<T>(T message) where T : class;
    }
}
