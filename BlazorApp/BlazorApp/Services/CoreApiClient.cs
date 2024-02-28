using LetsGame.Common.Data.Entities.Events;

namespace BlazorApp.Services;

public class CoreApiClient (HttpClient client)
{
    public async Task<LGEvent[]> GetEvents() =>
        await client.GetFromJsonAsync<LGEvent[]>("events") ?? [];
}