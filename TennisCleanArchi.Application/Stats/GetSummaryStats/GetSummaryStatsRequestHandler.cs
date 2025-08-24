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
        // Try to get the stats from the cache, if not present compute and cache them for 10 minutes
        var cachedStats = await _cachingService.GetOrAddAsync("SummaryStats",
            async () => await ComputeStats(cancellationToken), TimeSpan.FromMinutes(10));

        return cachedStats;
    }

    private async Task<SummaryStatsDto> ComputeStats(CancellationToken cancellationToken)
    {
        var bestWinRatioCountry = await _dbContext.Countries
            .Include(c => c.Players)
            .OrderByDescending(c => c.Players.Average(p => p.Data.Last.Length != 0 ? (double)p.Data.Last.Sum() / p.Data.Last.Length : 0))
            .Select(c => c.Adapt<CountryDto>())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        var averageImc = await _dbContext.Players
            .Select(p => (p.Data.Weight / 1000.0) / Math.Pow(p.Data.Height / 100.0, 2))
            .AverageAsync(cancellationToken);

        var heightMedian = await GetHeightMedianAsync(cancellationToken);

        return new SummaryStatsDto
        {
            BestCountryByWinRatio = bestWinRatioCountry,
            PlayersAverageImc = Math.Round(averageImc, 2),
            PlayersHeightMean = Math.Round(heightMedian, 2)
        };
    }

    private async Task<double> GetHeightMedianAsync(CancellationToken cancellationToken)
    {
        var count = await _dbContext.Players
            .Where(p => p.Data.Height > 0)
            .CountAsync(cancellationToken);

        double heightMedian = 0;

        IQueryable<double> query = _dbContext.Players
                    .OrderBy(p => p.Data.Height)
                    .Select(p => (double)p.Data.Height);

        if (count > 0)
        {
            if (count % 2 == 1)
            {
                heightMedian = await query
                    .Skip(count / 2)
                    .FirstAsync(cancellationToken);
            }
            else
            {
                var twoMiddleAverage = await query
                    .Skip(count / 2 - 1)
                    .Take(2)
                    .AverageAsync(cancellationToken);

                heightMedian = twoMiddleAverage;
            }
        }

        return heightMedian;
    }
}
