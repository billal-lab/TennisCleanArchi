using Microsoft.EntityFrameworkCore;
using TennisCleanArchi.Application.Common.Data;
using TennisCleanArchi.Domain;
using TennisCleanArchi.Shared;

namespace TennisCleanArchi.Infrastructure.Persistance;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Player> Players { get; set; }

    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Country)
                    .WithMany(e => e.Players)
                    .HasForeignKey(e => e.CountryCode)
                    .IsRequired();

            // Configure the owned type to be stored as JSON
            entity.OwnsOne(e => e.Data, builder =>
            {
                builder.ToJson();
            });

            // Configure conversion from string to value object and vice versa
            entity
                .Property(entity => entity.Sex)
                .HasConversion(
                        v => v.Value,
                        value => Sex.FromValue(value));
        });

        base.OnModelCreating(modelBuilder);
    }
}
