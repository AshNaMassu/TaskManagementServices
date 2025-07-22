using Confluent.Kafka;

namespace Infrastructure.Configurations
{
    /// <summary>
    /// Базовый класс настроек для подключения к инстансу Kafka
    /// </summary>
    public abstract class KafkaConfigurationBase : ClientConfig
    {
        /// <summary>
        /// Проверка правильности заданных базовых настроек Kafka
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(BootstrapServers);
            }
        }
    }
}
