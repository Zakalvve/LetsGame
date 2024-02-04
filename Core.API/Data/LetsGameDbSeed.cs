using Core.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.API.Data;

public class LetsGameDbSeed(ILogger<LetsGameDbSeed> logger) : IDbSeeder<LetsGameContext>
{
    public async Task SeedAsync(LetsGameContext context)
    {
        List<LGEvent> events = new List<LGEvent>
        {
            new LGEvent
            {
                Name = "Event 1",
            },
            new LGEvent
            {
                Name = "Event 2"
            }
        };

        await context.AddRangeAsync(events);
        await context.SaveChangesAsync();


        List<LGPoll> polls = new List<LGPoll>
        {
            new LGPoll
            {
                Title = "Poll 1"
            }
        };

        await context.AddRangeAsync(polls);
        await context.SaveChangesAsync();
    }
}
