using System.ComponentModel.DataAnnotations;

namespace Reactivities.Domain.Models
{
    public class Activity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }

        [Required]
        [StringLength(20)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Venue { get; set; } = string.Empty;

        [Required]
        public bool IsCancelled { get; set; }

        public ICollection<ActivityAttendee> Attendees { get; set; } = new List<ActivityAttendee>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
