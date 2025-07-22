using Infrastructure.Configurations;

namespace Infrastructure.Interfaces
{
    /// <summary>
    /// Сервис проверки состояния подключения к Kafka
    /// </summary>
    /// <typeparam name="TKafkaConfiguration">Тип конфигурации Kafka</typeparam>
    /// <typeparam name="TConfigTopic">Тип конфигурации топика</typeparam>
    public interface IKafkaInfoService<TKafkaConfiguration, TConfigTopic>
        where TKafkaConfiguration : KafkaConfigurationBase
        where TConfigTopic : class, ITopicConfiguration
    {
        /// <summary>
        /// Проверяет доступность Kafka
        /// </summary>
        /// <returns>
        /// Кортеж (result: результат проверки, errorMessage: сообщение об ошибке)
        /// </returns>
        Task<(bool result, string errorMessage)> HealthCheckAsync();
    }
}
