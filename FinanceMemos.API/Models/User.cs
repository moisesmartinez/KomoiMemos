namespace FinanceMemos.API.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}
