using Microsoft.EntityFrameworkCore;
using TennisCleanArchi.Domain;

namespace TennisCleanArchi.Application.Data;

public interface IApplicationDbContext
{
    DbSet<Player> Players { get; set; }

    DbSet<Country> Countries { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
