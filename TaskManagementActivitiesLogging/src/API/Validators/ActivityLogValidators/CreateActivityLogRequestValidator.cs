using Application.DTO.ActivityLog;
using FluentValidation;

namespace API.Validators.ActivityLogValidators
{
    public class CreateActivityLogRequestValidator : AbstractValidator<CreateActivityLogRequest>
    {
        public CreateActivityLogRequestValidator()
        {
            RuleFor(x => x.EventType).NotEmpty();
            RuleFor(x => x.Entity).NotEmpty();
            RuleFor(x => x.EntityId).GreaterThan(0);
        }
    }
}
