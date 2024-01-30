using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Services;

public class CoreApiClient (HttpClient client)
{
    public async Task<LGEvent[]> GetEvents() =>
        await client.GetFromJsonAsync<LGEvent[]>("events") ?? [];
}

public class LGEvent
{
    public string Name { get; set; }
}