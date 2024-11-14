using System.ComponentModel.DataAnnotations;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Account.Models
{
    public class RefreshToken
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Token { get; set; }

        [Required]
        public required DateTime Expires { get; set; }

        public DateTime? Revoked { get; set; }

        public required AppUser User { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
