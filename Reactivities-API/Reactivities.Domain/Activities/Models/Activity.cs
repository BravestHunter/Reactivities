using System.ComponentModel.DataAnnotations;
using Reactivities.Domain.Models;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Activities.Models
{
    public class Activity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [StringLength(400)]
        public string? Description { get; set; }

        [Required]
        [StringLength(20)]
        public required string Category { get; set; }

        [Required]
        [StringLength(100)]
        public required string City { get; set; }

        [Required]
        [StringLength(100)]
        public required string Venue { get; set; }

        [Required]
        public bool IsCancelled { get; set; }

        public required AppUser Host { get; set; }
        public ICollection<ActivityAttendee> Attendees { get; set; } = new List<ActivityAttendee>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
