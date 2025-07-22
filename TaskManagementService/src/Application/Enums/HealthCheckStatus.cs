namespace Application.Enums
{
    /// <summary>
    /// Результат проверки состояния сервиса
    /// </summary>
    public enum HealthCheckStatus
    {
        /// <summary>
        /// Сервис работает нормально
        /// </summary>
        Ok,

        /// <summary>
        /// Обнаружены проблемы в работе сервиса
        /// </summary>
        Failed
    }
}
