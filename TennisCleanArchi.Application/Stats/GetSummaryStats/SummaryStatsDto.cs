using TennisCleanArchi.Application.Countries.ListCountries;

namespace TennisCleanArchi.Application.Stats.GetSummaryStats;

public record SummaryStatsDto
{
    public CountryDto? BestCountryByWinRatio { get; set; }
    public double PlayersHeightMean { get; set; }
    public double PlayersAverageImc { get; set; }
}
