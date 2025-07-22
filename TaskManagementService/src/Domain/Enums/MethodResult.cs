namespace Domain.Enums
{
    /// <summary>
    /// Типы результатов выполнения операций
    /// </summary>
    public enum MethodResultType
    {
        /// <summary>
        /// Операция выполнена успешно
        /// </summary>
        Success,

        /// <summary>
        /// Ошибка валидации входных данных
        /// </summary>
        ValidationError,

        /// <summary>
        /// Внутренняя ошибка сервера
        /// </summary>
        InternalError,

        /// <summary>
        /// Запрашиваемый ресурс не найден
        /// </summary>
        NotFound
    }
}
