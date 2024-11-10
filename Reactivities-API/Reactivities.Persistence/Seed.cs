using Microsoft.AspNetCore.Identity;
using Reactivities.Domain.Activities.Models;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Persistence
{
    internal class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any() && !context.Activities.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        DisplayName = "Bob",
                        UserName = "bob",
                        Email = "bob@test.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Jane",
                        UserName = "jane",
                        Email = "jane@test.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Tom",
                        UserName = "tom",
                        Email = "tom@test.com"
                    },
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }

                var userFollowings = new List<UserFollowing>()
                {
                    new UserFollowing()
                    {
                        Observer = users[0],
                        Target = users[1]
                    },
                    new UserFollowing()
                    {
                        Observer = users[1],
                        Target = users[0]
                    },
                    new UserFollowing()
                    {
                        Observer = users[2],
                        Target = users[0]
                    },
                    new UserFollowing()
                    {
                        Observer = users[2],
                        Target = users[1]
                    }
                };
                await context.UserFollowings.AddRangeAsync(userFollowings);

                var activities = new List<Activity>
                {
                    new Activity
                    {
                        Title = "Past Activity 1",
                        Date = DateTime.UtcNow.AddMonths(-2),
                        Description = "Activity 2 months ago",
                        Category = "drinks",
                        City = "London",
                        Venue = "Pub",
                        Host = users[0]
                    },
                    new Activity
                    {
                        Title = "Past Activity 2",
                        Date = DateTime.UtcNow.AddMonths(-1),
                        Description = "Activity 1 month ago",
                        Category = "culture",
                        City = "Paris",
                        Venue = "The Louvre",
                        Host = users[0]
                    },
                    new Activity
                    {
                        Title = "Future Activity 1",
                        Date = DateTime.UtcNow.AddMonths(1),
                        Description = "Activity 1 month in future",
                        Category = "music",
                        City = "London",
                        Venue = "Wembly Stadium",
                        Host =  users[2]
                    },
                    new Activity
                    {
                        Title = "Future Activity 2",
                        Date = DateTime.UtcNow.AddMonths(2),
                        Description = "Activity 2 months in future",
                        Category = "food",
                        City = "London",
                        Venue = "Jamies Italian",
                        Host = users[0]
                    },
                    new Activity
                    {
                        Title = "Future Activity 3",
                        Date = DateTime.UtcNow.AddMonths(3),
                        Description = "Activity 3 months in future",
                        Category = "drinks",
                        City = "London",
                        Venue = "Pub",
                        Host = users[1]
                    },
                    new Activity
                    {
                        Title = "Future Activity 4",
                        Date = DateTime.UtcNow.AddMonths(4),
                        Description = "Activity 4 months in future",
                        Category = "culture",
                        City = "London",
                        Venue = "British Museum",
                        Host = users[1]
                    },
                    new Activity
                    {
                        Title = "Future Activity 5",
                        Date = DateTime.UtcNow.AddMonths(5),
                        Description = "Activity 5 months in future",
                        Category = "drinks",
                        City = "London",
                        Venue = "Punch and Judy",
                        Host = users[0]
                    },
                    new Activity
                    {
                        Title = "Future Activity 6",
                        Date = DateTime.UtcNow.AddMonths(6),
                        Description = "Activity 6 months in future",
                        Category = "music",
                        City = "London",
                        Venue = "O2 Arena",
                        Host = users[2]
                    },
                    new Activity
                    {
                        Title = "Future Activity 7",
                        Date = DateTime.UtcNow.AddMonths(7),
                        Description = "Activity 7 months in future",
                        Category = "travel",
                        City = "Berlin",
                        Venue = "All",
                        Host = users[0]
                    },
                    new Activity
                    {
                        Title = "Future Activity 8",
                        Date = DateTime.UtcNow.AddMonths(8),
                        Description = "Activity 8 months in future",
                        Category = "drinks",
                        City = "London",
                        Venue = "Pub",
                        Host = users[2]
                    }
                };

                activities[1].Attendees.Add(new ActivityAttendee() { User = users[1], Activity = activities[1] });
                activities[2].Attendees.Add(new ActivityAttendee() { User = users[1], Activity = activities[2] });
                activities[3].Attendees.Add(new ActivityAttendee() { User = users[2], Activity = activities[3] });
                activities[4].Attendees.Add(new ActivityAttendee() { User = users[0], Activity = activities[4] });
                activities[6].Attendees.Add(new ActivityAttendee() { User = users[1], Activity = activities[6] });
                activities[6].Attendees.Add(new ActivityAttendee() { User = users[2], Activity = activities[6] });
                activities[7].Attendees.Add(new ActivityAttendee() { User = users[1], Activity = activities[7] });
                activities[8].Attendees.Add(new ActivityAttendee() { User = users[2], Activity = activities[8] });
                activities[9].Attendees.Add(new ActivityAttendee() { User = users[1], Activity = activities[9] });

                await context.Activities.AddRangeAsync(activities);

                await context.SaveChangesAsync();
            }
        }
    }
}
