namespace Reactivities.Domain.Users.Models
{
    public class UserFollowing
    {
        public long ObserverId { get; set; }
        public required AppUser Observer { get; set; }
        public long TargetId { get; set; }
        public required AppUser Target { get; set; }
    }
}
