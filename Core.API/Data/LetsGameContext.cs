using Microsoft.EntityFrameworkCore;
using Core.API.Models;

namespace Core.API.Data;

public class LetsGameContext : DbContext
{
    public LetsGameContext(DbContextOptions<LetsGameContext> options, IConfiguration config) : base(options) { }

    public DbSet<LGEvent> LGEvents { get; set; }
    public DbSet<LGPoll> LGPolls { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<LGEvent>(b => {
            b.Property(b => b.DateAdded)
                .HasDefaultValueSql("getdate()");

            b.Property(b => b.LastModified)
                .HasDefaultValueSql("getdate()");
        });

        builder.Entity<LGPoll>(b => {
            b.Property(b => b.DateAdded)
                .HasDefaultValueSql("getdate()");

            b.Property(b => b.LastModified)
                .HasDefaultValueSql("getdate()");
        });
    }
}
