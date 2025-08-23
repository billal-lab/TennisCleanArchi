using TennisCleanArchi.Application.Countries;

namespace TennisCleanArchi.Application.Stats;

public class StatsDto
{
    public CountryDto? BestCountryByWinRatio { get; set; }
    public double PlayersHeightMean { get; set; }
    public double PlayersAverageImc { get; set; }
}
