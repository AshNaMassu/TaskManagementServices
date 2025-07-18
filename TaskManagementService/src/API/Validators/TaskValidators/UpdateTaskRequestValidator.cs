using Application.DTO.Task;
using Domain.Enums;
using FluentValidation;

namespace API.Validators.ActivityLogValidators
{
    public class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
    {
        public UpdateTaskRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();

            RuleFor(x => x.Status)
                .NotEmpty()
                .Must(authEventType => Enum.GetNames(typeof(Status)).Any(e => e.Equals(authEventType)))
                .WithMessage($"Invalid Status. Valid values are: {string.Join(", ", Enum.GetNames(typeof(Status)))}"); ;
        }
    }
}
