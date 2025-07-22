using Application.DTO.Result;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Базовый контроллер для обработки результатов методов
    /// </summary>
    /// <typeparam name="TService">Тип сервиса</typeparam>
    public class BaseController<TService> : ControllerBase
    {
        protected readonly TService _service;

        /// <summary>
        /// Инициализирует базовый контроллер
        /// </summary>
        public BaseController(TService service)
        {
            _service = service;
        }

        /// <summary>
        /// Обрабатывает результат операции без возвращаемого значения
        /// </summary>
        protected IActionResult GetResponse(MethodResult methodResult, MethodType? type = null)
        {
            if (methodResult == null)
            {
                return BadRequest();
            }

            return methodResult.ResultType switch
            {
                MethodResultType.Success => type.HasValue ? type.Value == MethodType.Create ? Created() : NoContent() : Ok(),
                MethodResultType.ValidationError => BadRequest(methodResult.Error),
                MethodResultType.NotFound => NotFound(methodResult.Error),
                MethodResultType.InternalError => StatusCode(500, methodResult.Error),
                _ => StatusCode(500, "Unknown error type")
            };
        }

        /// <summary>
        /// Обрабатывает результат операции с возвращаемым значением
        /// </summary>
        protected IActionResult GetResponse<T>(MethodResult<T> methodResult, MethodType? type = null)
        {
            if (methodResult == null)
            {
                return BadRequest();
            }

            return methodResult.ResultType switch
            {
                MethodResultType.Success => type.HasValue ? type.Value == MethodType.Create ? Created() : NoContent() : Ok(methodResult.Value),
                MethodResultType.ValidationError => BadRequest(methodResult.Error),
                MethodResultType.NotFound => NotFound(methodResult.Error),
                MethodResultType.InternalError => StatusCode(500, methodResult.Error),
                _ => StatusCode(500, "Unknown error type")
            };
        }

        /// <summary>
        /// Тип HTTP-метода
        /// </summary>
        protected enum MethodType
        {
            /// <summary>
            /// Создание ресурса
            /// </summary>
            Create,
            /// <summary>
            /// Удаление ресурса
            /// </summary>
            Delete
        }
    }
}
