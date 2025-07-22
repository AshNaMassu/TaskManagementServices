using Domain.Enums;

namespace Application.DTO.Result
{
    /// <summary>
    /// Результат выполнения метода
    /// </summary>
    /// <remarks>
    /// Используется для стандартизированного возврата результатов и ошибок
    /// </remarks>
    public class MethodResult
    {
        /// <summary>
        /// Тип результата операции
        /// </summary>
        public MethodResultType ResultType { get; }

        /// <summary>
        /// Сообщение об ошибке (заполняется только при ошибках)
        /// </summary>
        /// <example>Задача с ID 123 не найдена</example>
        public string Error { get; }

        protected MethodResult(MethodResultType resultType, string error)
        {
            ResultType = resultType;
            Error = error;
        }

        /// <summary>
        /// Создает результат с ошибкой валидации
        /// </summary>
        /// <param name="error">Сообщение об ошибке</param>
        /// <example>MethodResult.ValidationError("Название задачи обязательно")</example>
        public static MethodResult ValidationError(string error)
        {
            return new MethodResult(MethodResultType.ValidationError, error);
        }

        /// <summary>
        /// Создает результат с ошибкой "Не найдено"
        /// </summary>
        /// <param name="error">Описание отсутствующего ресурса</param>
        /// <example>MethodResult.NotFound("Задача не найдена")</example>
        public static MethodResult NotFound(string error)
        {
            return new MethodResult(MethodResultType.NotFound, error);
        }

        /// <summary>
        /// Создает результат с внутренней ошибкой сервера
        /// </summary>
        /// <param name="error">Техническое сообщение об ошибке</param>
        /// <example>MethodResult.InternalError("Ошибка подключения к БД")</example>
        public static MethodResult InternalError(string error)
        {
            return new MethodResult(MethodResultType.InternalError, error);
        }

        /// <summary>
        /// Создает успешный результат без возвращаемого значения
        /// </summary>
        /// <returns>MethodResult.Success()</returns>
        public static MethodResult Success()
        {
            return new MethodResult(MethodResultType.Success, null);
        }
    }

    /// <summary>
    /// Результат выполнения метода с возвращаемым значением
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого значения</typeparam>
    public class MethodResult<T> : MethodResult
    {
        /// <summary>
        /// Возвращаемое значение (только для успешного результата)
        /// </summary>
        public T Value { get; }

        protected MethodResult(MethodResultType resultType, string error, T value) : base(resultType, error)
        {
            Value = value;
        }

        /// <summary>
        /// Создаёт результат с ошибкой валидации
        /// </summary>
        /// <param name="error">Сообщение об ошибке</param>
        /// <returns></returns>
        public new static MethodResult<T> ValidationError(string error)
        {
            return new MethodResult<T>(MethodResultType.ValidationError, error, default);
        }

        /// <summary>
        /// Создаёт результат с ошибкой "Не найдено"
        /// </summary>
        /// <param name="error">Описание отсутствующего ресурса</param>
        /// <returns></returns>
        public new static MethodResult<T> NotFound(string error)
        {
            return new MethodResult<T>(MethodResultType.NotFound, error, default);
        }

        /// <summary>
        /// Создает резульат с внутренней ошибкой сервера
        /// </summary>
        /// <param name="error">Техническое сообщение об ошибке</param>
        /// <returns></returns>
        public new static MethodResult<T> InternalError(string error)
        {
            return new MethodResult<T>(MethodResultType.InternalError, error, default);
        }

        /// <summary>
        /// Создает успешный результат с возвращаемым значением
        /// </summary>
        /// <param name="value">Результат выполнения операции</param>
        /// <returns></returns>
        public static MethodResult<T> Success(T value)
        {
            return new MethodResult<T>(MethodResultType.Success, null, value);
        }
    }
}
