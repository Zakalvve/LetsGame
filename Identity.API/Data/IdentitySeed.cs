using Identity.API.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data;

public class IdentitySeed(ILogger<IdentitySeed> logger, UserManager<ApplicationUser> userManager) : IDbSeeder<ApplicationContext>
{
    public async Task SeedAsync(ApplicationContext context)
    {
        if (context.Users.Count() == 0)
        {
            var alice = await userManager.FindByNameAsync("alice");

            if (alice == null)
            {
                alice = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Alice",
                    LastName = "Smith",
                    UserName = "alice",
                    Email = "AliceSmith@email.com",
                    EmailConfirmed = true,
                    PhoneNumber = "1234567890",
                };

                var result = userManager.CreateAsync(alice, "Pass123$").Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("alice created");
                }
            }
            else
            {
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("alice already exists");
                }
            }

            var bob = await userManager.FindByNameAsync("bob");

            if (bob == null)
            {
                bob = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Bob",
                    LastName = "Smith",
                    UserName = "bob",
                    Email = "BobSmith@email.com",
                    EmailConfirmed = true,                    
                    PhoneNumber = "1234567890",
                };

                var result = await userManager.CreateAsync(bob, "Pass123$");

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("bob created");
                }
            }
            else
            {
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("bob already exists");
                }
            }
        }

        if (context.ProfilePictures.Count() == 0)
        {

            List<ProfilePicture> profilePictures = new List<ProfilePicture>
            {
                new ProfilePicture()
                {
                    imagePath = "default-profile-pictures/default-profile-pictures-1.jpg"
                },
                new ProfilePicture()
                {
                    imagePath = "default-profile-pictures/default-profile-pictures-2.jpg"
                },
                new ProfilePicture()
                {
                    imagePath = "default-profile-pictures/default-profile-pictures-3.jpg"
                },
                new ProfilePicture()
                {
                    imagePath = "default-profile-pictures/default-profile-pictures-4.jpg"
                }
            };

            await context.AddRangeAsync(profilePictures);
            await context.SaveChangesAsync();
        }
    }
}
