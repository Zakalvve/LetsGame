using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Identity.API.Models.Data;
using LetsGame.Common.Data.Entities.Core;

namespace Identity.API.Data;

/// <remarks>
/// Add migrations using the following command inside the 'Identity.API' project directory:
///
/// dotnet ef migrations add [migration-name]
/// </remarks>

public class ApplicationContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    public virtual DbSet<ProfilePicture> ProfilePictures { get; set; }
    public virtual DbSet<SystemType> SystemTypes { get; set; }
    public virtual DbSet<UserRelationship> UserRelationships { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(b => {
            b.Property(b => b.DateAdded)
                .HasDefaultValueSql("getdate()");
            b.Property(b => b.LastModified)
                .HasDefaultValueSql("getdate()");

            b.Ignore(user => user.Friends);
        });

        builder.Entity<UserRelationship>(b =>
        {
            b.Property(b => b.DateAdded)
                .HasDefaultValueSql("getdate()");
            b.Property(b => b.LastModified)
                .HasDefaultValueSql("getdate()");

            b.Property(b => b.PendingAccept)
                .HasDefaultValue(true);

            b.HasOne(ship => ship.Requester)
                .WithMany(user => user.RelationshipsAsRequester)
                .HasForeignKey(ship => ship.RequesterId)
                .OnDelete(DeleteBehavior.NoAction);

            b.HasOne(ship => ship.Addressee)
                .WithMany(user => user.RelationshipsAsAddressee)
                .HasForeignKey(ship => ship.AddresseeId)
                .OnDelete(DeleteBehavior.NoAction);

            b.HasKey(k => new { k.RequesterId, k.AddresseeId });
        });

        builder.Entity<ProfilePicture>(b => {
            b.Property(b => b.DateAdded)
                .HasDefaultValueSql("getdate()");
            b.Property(b => b.LastModified)
                .HasDefaultValueSql("getdate()");
        });

        builder.Entity<SystemType>(b =>
        {
            b.Property(b => b.DateAdded)
                .HasDefaultValueSql("getdate()");
            b.Property(b => b.LastModified)
                .HasDefaultValueSql("getdate()");
        });

        builder.Entity<SystemTypeValue>(b =>
        {
            b.Property(b => b.DateAdded)
                .HasDefaultValueSql("getdate()");
            b.Property(b => b.LastModified)
                .HasDefaultValueSql("getdate()");
        });
    }
}
