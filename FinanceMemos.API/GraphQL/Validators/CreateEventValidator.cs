using FinanceMemos.API.GraphQL.Inputs;
using FluentValidation;

namespace FinanceMemos.API.GraphQL.Validators
{
    public class CreateEventValidator : AbstractValidator<CreateEventInput>
    {
        public CreateEventValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(5).WithMessage("Event Name must be at least 5 characters long.")
                .MaximumLength(100).WithMessage("Event Name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
