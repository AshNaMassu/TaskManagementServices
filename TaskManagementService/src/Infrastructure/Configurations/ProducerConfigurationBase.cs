using Confluent.Kafka;
using Infrastructure.Interfaces;

namespace Infrastructure.Configurations
{
    /// <summary>
    /// Базовый класс настроек продюсера Kafka
    /// </summary>
    public abstract class ProducerConfigurationBase : ProducerConfig, ITopicConfiguration
    {
        /// <summary>
        /// Название топика для отправки сообщений продюсером Kafka
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Проверка правильности заданных базовых настроек продюсера Kafka
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(Topic);
            }
        }
    }
}
