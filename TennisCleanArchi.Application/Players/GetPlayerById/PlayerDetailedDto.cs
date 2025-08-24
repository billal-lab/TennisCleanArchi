using TennisCleanArchi.Application.Countries.ListCountries;
using TennisCleanArchi.Application.Players.ListPlayers;
using TennisCleanArchi.Domain;

namespace TennisCleanArchi.Application.Players.GetPlayerById;

public record PlayerDetailedDto : PlayerDto
{
    public CountryDto Country { get; set; }
    public PlayerStats Data { get; set; }
}
