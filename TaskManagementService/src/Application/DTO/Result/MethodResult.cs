using Domain.Enums;

namespace Application.DTO.Result
{
    /// <summary>
    /// Результат выполнения метода
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого значения</typeparam>
    public class MethodResult
    {
        public MethodResultType ResultType { get; }
        public string Error { get; }

        protected MethodResult(MethodResultType resultType, string error)
        {
            ResultType = resultType;
            Error = error;
        }

        public static MethodResult ValidationError(string error)
        {
            return new MethodResult(MethodResultType.ValidationError, error);
        }

        public static MethodResult NotFound(string error)
        {
            return new MethodResult(MethodResultType.NotFound, error);
        }

        public static MethodResult InternalError(string error)
        {
            return new MethodResult(MethodResultType.InternalError, error);
        }

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

        public new static MethodResult<T> ValidationError(string error)
        {
            return new MethodResult<T>(MethodResultType.ValidationError, error, default);
        }

        public new static MethodResult<T> NotFound(string error)
        {
            return new MethodResult<T>(MethodResultType.NotFound, error, default);
        }

        public new static MethodResult<T> InternalError(string error)
        {
            return new MethodResult<T>(MethodResultType.InternalError, error, default);
        }

        public static MethodResult<T> Success(T value)
        {
            return new MethodResult<T>(MethodResultType.Success, null, value);
        }
    }
}
