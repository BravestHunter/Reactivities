using System.ComponentModel.DataAnnotations;
using Reactivities.Domain.Activities.Models;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Models
{
    public class Comment
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(300)]
        public required string Body { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public required AppUser Author { get; set; }
        public required Activity Activity { get; set; }
    }
}
