namespace FinanceMemos.API.Models;

public partial class Expense
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public decimal Amount { get; set; }

    public string Category { get; set; } = null!;

    public DateTime Date { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
