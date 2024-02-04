using Microsoft.EntityFrameworkCore;
using Core.API.Models;

namespace Core.API.Data;

public class LetsGameContext : DbContext
{
    public LetsGameContext(DbContextOptions<LetsGameContext> options, IConfiguration config) : base(options) { }

    public DbSet<LGEvent> LGEvents { get; set; }
    public DbSet<LGPoll> LGPolls { get; set; }
}
