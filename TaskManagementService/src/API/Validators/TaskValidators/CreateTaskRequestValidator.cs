using Application.DTO.Task;
using FluentValidation;

namespace API.Validators.ActivityLogValidators
{
    /// <summary>
    /// Валидатор для запроса создания задачи
    /// </summary>
    public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
    {
        public CreateTaskRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
