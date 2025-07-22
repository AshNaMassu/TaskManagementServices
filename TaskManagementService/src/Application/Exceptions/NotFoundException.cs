namespace Application.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при отсутствии запрашиваемого ресурса
    /// </summary>
    /// <remarks>
    /// Генерируется при попытке доступа к несуществующей задаче.
    /// Преобразуется в HTTP 404 ответ.
    /// </remarks>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Создает исключение с сообщением по умолчанию
        /// </summary>
        public NotFoundException() : base("Not found") { }

        /// <summary>
        /// Создает исключение с кастомным сообщением
        /// </summary>
        /// <param name="message">Описание ошибки</param>
        /// <example>new NotFoundException("Задача с ID 123 не найдена")</example>
        public NotFoundException(string message)
            : base(message) { }
    }
}
