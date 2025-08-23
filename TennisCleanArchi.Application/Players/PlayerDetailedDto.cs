using TennisCleanArchi.Application.Countries;
using TennisCleanArchi.Domain;

namespace TennisCleanArchi.Application.Players;

public class PlayerDetailedDto : PlayerDto
{
    public CountryDto Country { get; set; }
    public PlayerStats Data { get; set; }
}
