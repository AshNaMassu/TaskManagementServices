using Application.DTO.Task;
using Domain.Enums;
using FluentValidation;

namespace API.Validators.ActivityLogValidators
{
    /// <summary>
    /// Валидатор для запроса обновления статуса задачи
    /// </summary>
    public class UpdateTaskStatusRequestValidator : AbstractValidator<UpdateTaskStatusRequest>
    {
        public UpdateTaskStatusRequestValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty()
                .Must(authEventType => Enum.GetNames(typeof(Status)).Any(e => e.Equals(authEventType)))
                .WithMessage($"Invalid Status. Valid values are: {string.Join(", ", Enum.GetNames(typeof(Status)))}"); ;
        }
    }
}
