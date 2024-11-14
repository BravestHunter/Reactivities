using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Activities.Models
{
    public class ActivityAttendee
    {
        public long AppUserId { get; set; }
        public required AppUser User { get; set; }
        public long ActivityId { get; set; }
        public required Activity Activity { get; set; }
    }
}
