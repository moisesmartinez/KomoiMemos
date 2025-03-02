using System.ComponentModel.DataAnnotations;

namespace FinanceMemos.API.Models;

public partial class Image
{
    public int Id { get; set; }

    public int NoteId { get; set; }

    [Required(ErrorMessage = "Image URL is required.")]
    [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters.")]
    public string ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Note Note { get; set; } = null!;
}
