using FluentValidation;

namespace FinanceMemos.API.Models;

public partial class Event
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}

public class EventValidator : AbstractValidator<Event>
{
    public EventValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Event name is required.")
            .Length(3, 100).WithMessage("Event name must be between 3 and 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}
