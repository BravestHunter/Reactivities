using System.ComponentModel.DataAnnotations;

namespace Reactivities.Domain.Models
{
    public class Comment
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Body { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public AppUser Author { get; set; }
        public Activity Activity { get; set; }
    }
}
