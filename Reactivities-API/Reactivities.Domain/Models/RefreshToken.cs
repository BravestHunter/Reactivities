using System.ComponentModel.DataAnnotations;

namespace Reactivities.Domain.Models
{
    public class RefreshToken
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Token { get; set; } = string.Empty;

        [Required]
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddMonths(1);

        public DateTime? Revoked { get; set; }

        public AppUser AppUser { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
