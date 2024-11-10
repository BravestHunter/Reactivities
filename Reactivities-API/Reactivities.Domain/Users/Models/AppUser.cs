using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Reactivities.Domain.Activities.Models;
using Reactivities.Domain.Models;
using Reactivities.Domain.Photos.Models;

namespace Reactivities.Domain.Users.Models
{
    public class AppUser : IdentityUser<long>
    {
        [Required]
        [StringLength(20)]
        public string DisplayName { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Bio { get; set; }

        public ICollection<ActivityAttendee> Activities { get; set; } = new List<ActivityAttendee>();
        public ICollection<Photo> Photos { get; set; } = new List<Photo>();
        public ICollection<UserFollowing> Followings { get; set; } = new List<UserFollowing>();
        public ICollection<UserFollowing> Followers { get; set; } = new List<UserFollowing>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
