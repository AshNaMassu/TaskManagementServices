using Application.DTO.Result;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BaseController<TService> : ControllerBase
    {
        protected readonly TService _service;

        public BaseController(TService service)
        {
            _service = service;
        }

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

        protected IActionResult GetResponse<T>(MethodResult<T> methodResult, MethodType? type = null)
        {
            if (methodResult == null)
            {
                return BadRequest();
            }

            return methodResult.ResultType switch
            {
                MethodResultType.Success => type.HasValue ? type.Value == MethodType.Create ? Created(methodResult.Value) : NoContent() : Ok(methodResult.Value),
                MethodResultType.ValidationError => BadRequest(methodResult.Error),
                MethodResultType.NotFound => NotFound(methodResult.Error),
                MethodResultType.InternalError => StatusCode(500, methodResult.Error),
                _ => StatusCode(500, "Unknown error type")
            };
        }

        protected enum MethodType
        {
            Create,
            Delete
        }
    }
}
