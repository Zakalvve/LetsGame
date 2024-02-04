using Identity.API.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data;

public class ProfilePictureSeed(ILogger<ProfilePictureSeed> logger) : IDbSeeder<ApplicationContext>
{
    public async Task SeedAsync(ApplicationContext context)
    {
        List<ProfilePicture> profilePictures = new List<ProfilePicture>
        {
            new ProfilePicture()
            {
                imagePath = "default-profile-pictures-1"
            },
            new ProfilePicture()
            {
                imagePath = "default-profile-pictures-2"
            },
            new ProfilePicture()
            {
                imagePath = "default-profile-pictures-3"
            },
            new ProfilePicture()
            {
                imagePath = "default-profile-pictures-4"
            }
        };

        await context.AddRangeAsync(profilePictures);
        await context.SaveChangesAsync();
    }
}
