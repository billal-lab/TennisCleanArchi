using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TennisCleanArchi.Application.Common.Caching;
using TennisCleanArchi.Application.Common.Data;
using TennisCleanArchi.Application.Countries.ListCountries;
using TennisCleanArchi.Application.Stats.GetSummaryStats;

namespace TennisCleanArchi.Application.Stats.GetStats;
public class GetSummaryStatsRequestHandler : IRequestHandler<GetSummaryStatsRequest, SummaryStatsDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ICachingService _cachingService;

    public GetSummaryStatsRequestHandler(IApplicationDbContext dbContext, ICachingService cachingService)
    {
        _dbContext = dbContext;
        _cachingService = cachingService;
    }

    public async Task<SummaryStatsDto> Handle(GetSummaryStatsRequest request, CancellationToken cancellationToken)
    {
        // Try to get the stats from the cache
        var cacheKey = "SummaryStats";

        var cachedStats = await _cachingService.GetOrAddAsync<SummaryStatsDto>(cacheKey,
            async () => await ComputeStats(cancellationToken), TimeSpan.FromMinutes(10));

        return cachedStats;
    }

    private async Task<SummaryStatsDto> ComputeStats(CancellationToken cancellationToken)
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
            .AverageAsync(p => p.Data.Weight / 1000 * Math.Pow(p.Data.Height / 100.0, 2), cancellationToken);

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

        return new SummaryStatsDto
        {
            BestCountryByWinRatio = bestWinRatioCountry,
            PlayersAverageImc = Math.Round(averageImc, 2),
            PlayersHeightMean = Math.Round(heightMedian, 2)
        };
    }
}
