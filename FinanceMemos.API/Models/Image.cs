namespace FinanceMemos.API.Models;

public partial class Image
{
    public int Id { get; set; }

    public int NoteId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    public virtual Note Note { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
