using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reactivities.Domain.Activities.Models;
using Reactivities.Domain.Models;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Persistence
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, long>
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserFollowing> UserFollowings { get; set; }

        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ActivityAttendee>(x => x.HasKey(aa => new { aa.AppUserId, aa.ActivityId }));

            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.User)
                .WithMany(a => a.Activities)
                .HasForeignKey(aa => aa.AppUserId);

            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.Activity)
                .WithMany(a => a.Attendees)
                .HasForeignKey(aa => aa.ActivityId);

            builder.Entity<Comment>()
                .HasOne(c => c.Activity)
                .WithMany(a => a.Comments)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserFollowing>(builder =>
            {
                builder.HasKey(k => new { k.ObserverId, k.TargetId });
                builder.HasOne(uf => uf.Observer)
                    .WithMany(o => o.Followings)
                    .HasForeignKey(uf => uf.ObserverId)
                    .OnDelete(DeleteBehavior.Cascade);
                builder.HasOne(uf => uf.Target)
                    .WithMany(o => o.Followers)
                    .HasForeignKey(uf => uf.TargetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
