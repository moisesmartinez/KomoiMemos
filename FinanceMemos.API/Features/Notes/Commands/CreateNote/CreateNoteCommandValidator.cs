using FluentValidation;

namespace FinanceMemos.API.Features.Notes.Commands.CreateNote
{
    public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
    {
        public CreateNoteCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is required.")
                .MaximumLength(50).WithMessage("Type must not exceed 50 characters.");

            RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("EventId must be greater than 0.");

            //RuleFor(x => x.UserId)
            //    .GreaterThan(0).WithMessage("UserId must be greater than 0.");
        }
    }
}
