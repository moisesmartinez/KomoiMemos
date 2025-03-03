namespace FinanceMemos.API.Models;

public partial class Note
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Type { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual User User { get; set; } = null!;
}
