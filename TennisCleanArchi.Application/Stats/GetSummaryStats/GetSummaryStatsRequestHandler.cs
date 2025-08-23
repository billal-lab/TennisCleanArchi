using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TennisCleanArchi.Application.Countries;
using TennisCleanArchi.Application.Data;

namespace TennisCleanArchi.Application.Stats.GetStats;
public class GetSummaryStatsRequestHandler : IRequestHandler<GetSummaryStatsRequest, StatsDto>
{
    private readonly IApplicationDbContext _dbContext;

    public GetSummaryStatsRequestHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    //TODO: Check performance

    public async Task<StatsDto> Handle(GetSummaryStatsRequest request, CancellationToken cancellationToken)
    {
        var bestWinRatioCountry = await _dbContext.Countries
            .AsNoTracking()
            .Include(c => c.Players)
            .OrderByDescending(c => c.Players.Average(p => p.Data.Last.Length != 0 ? (double)p.Data.Last.Sum() / p.Data.Last.Length : 0))
            .Select(c => c.Adapt<CountryDto>())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        var averageImc = await _dbContext.Players
            .AsNoTracking()
            .Where(p => p.Data.Height > 0 && p.Data.Weight > 0)
            .AverageAsync(p => p.Data.Weight / Math.Pow(p.Data.Height / 100.0, 2), cancellationToken);

        var count = await _dbContext.Players.Include(p => p.Country).CountAsync(cancellationToken);

        var heightMedian = await _dbContext.Players
            .AsNoTracking()
            .Where(p => p.Data.Height > 0)
            .OrderBy(p => p.Data.Height)
            .Select(p => p.Data.Height)
            .Skip((count - 1) / 2)
            .Take(1)
            .Select(h => (double)h)
            .FirstOrDefaultAsync(cancellationToken);

        return new StatsDto
        {
            BestCountryByWinRatio = bestWinRatioCountry,
            PlayersAverageImc = Math.Round(averageImc, 2),
            PlayersHeightMean = Math.Round(heightMedian, 2)
        };
    }
}
