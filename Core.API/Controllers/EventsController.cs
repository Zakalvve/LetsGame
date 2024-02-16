using Core.API.Data;
using Core.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Core.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController(LetsGameContext context, ILogger<EventsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<LGEvent>>> GetEvents()
    {
        var events = await context.LGEvents.ToListAsync();
        return Ok(events);
    }
}
