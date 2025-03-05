using FluentValidation;

namespace FinanceMemos.API.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(5).WithMessage("Event Name must be at least 5 characters long.")
                .MaximumLength(100).WithMessage("Event Name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            //RuleFor(x => x.UserId)
            //    .GreaterThan(0).WithMessage("UserId must be greater than 0.");
        }
    }
}
