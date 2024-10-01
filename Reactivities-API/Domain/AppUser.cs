using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public List<ActivityAttendee> Activities { get; set; } = new();
        public List<Photo> Photos { get; set; } = new();
        public List<UserFollowing> Followings { get; set; } = new();
        public List<UserFollowing> Followers { get; set; } = new();
        public List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}
