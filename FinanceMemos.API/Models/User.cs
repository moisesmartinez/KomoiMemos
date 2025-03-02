using System.ComponentModel.DataAnnotations;

namespace FinanceMemos.API.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(500)]
        public string PasswordHash { get; set; }
    }
}