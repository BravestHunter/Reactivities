namespace Reactivities.Persistence.Models
{
    public class UserFollowing
    {
        public long ObserverId { get; set; }
        public AppUser Observer { get; set; }
        public long TargetId { get; set; }
        public AppUser Target { get; set; }
    }
}
