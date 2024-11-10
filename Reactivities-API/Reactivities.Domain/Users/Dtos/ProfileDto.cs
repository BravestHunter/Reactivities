using Reactivities.Domain.Models;

namespace Reactivities.Domain.Users.Dtos
{
    public class ProfileDto
    {
        public required string Username { get; set; }
        public required string DisplayName { get; set; }
        public string? Bio { get; set; }
        public required string Image { get; set; }
        public bool Following { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    }
}
