using Reactivities.Domain.Models;

namespace Reactivities.Domain.Activities.Models
{
    public class ActivityAttendee
    {
        public long AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public long ActivityId { get; set; }
        public Activity Activity { get; set; }
    }
}
