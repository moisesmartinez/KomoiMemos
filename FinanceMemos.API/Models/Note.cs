using System.ComponentModel.DataAnnotations;

namespace FinanceMemos.API.Models;

public partial class Note
{
    public int Id { get; set; }

    public int EventId { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    public string Title { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Type is required.")]
    [StringLength(50, ErrorMessage = "Type cannot exceed 50 characters.")]
    public string Type { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Event Event { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}
