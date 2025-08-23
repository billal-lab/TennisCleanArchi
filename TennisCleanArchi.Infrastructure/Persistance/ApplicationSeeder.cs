using TennisCleanArchi.Domain;
using TennisCleanArchi.Shared;

namespace TennisCleanArchi.Infrastructure.Persistance;

public class ApplicationSeeder
{
    private readonly ApplicationDbContext _context;

    public ApplicationSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var srb = new Country { Picture = "https://tenisu.latelier.co/resources/Serbie.png", Code = "SRB" };
        var usa = new Country { Picture = "https://tenisu.latelier.co/resources/USA.png", Code = "USA" };
        var sui = new Country { Picture = "https://tenisu.latelier.co/resources/Suisse.png", Code = "SUI" };
        var esp = new Country { Picture = "https://tenisu.latelier.co/resources/Espagne.png", Code = "ESP" };
        var countries = new List<Country> { srb, usa, sui, esp };

        await _context.Countries.AddRangeAsync(countries);

        var players = new List<Player>
        {
            new Player
            {
                FirstName = "Novak",
                LastName = "Djokovic",
                ShortName = "N.DJO",
                Picture = "https://tenisu.latelier.co/resources/Djokovic.png",
                Country = srb,
                Sex = Sex.Male,
                Data = new PlayerStats
                {
                    Rank = 2,
                    Points = 2542,
                    Weight = 80000,
                    Height = 188,
                    Age = 31,
                    Last = [1, 1, 1, 1, 1]
                }
            },
            new Player
            {
                FirstName = "Venus",
                LastName = "Williams",
                ShortName = "V.WIL",
                Picture = "https://tenisu.latelier.co/resources/Venus.webp",
                Country = usa,
                Sex = Sex.Female,
                Data = new PlayerStats
                {
                    Rank = 52,
                    Points = 1105,
                    Weight = 74000,
                    Height = 185,
                    Age = 38,
                    Last = [0, 1, 0, 0, 1]
                }
            },
            new Player
            {
                FirstName = "Stan",
                LastName = "Wawrinka",
                ShortName = "S.WAW",
                Picture = "https://tenisu.latelier.co/resources/Wawrinka.png",
                Country = sui,
                Sex = Sex.Male,
                Data = new PlayerStats
                {
                    Rank = 21,
                    Points = 1784,
                    Weight = 81000,
                    Height = 183,
                    Age = 33,
                    Last = [1, 1, 1, 0, 1]
                }
            },
            new Player
            {
                FirstName = "Serena",
                LastName = "Williams",
                ShortName = "S.WIL",
                Picture = "https://tenisu.latelier.co/resources/Serena.png",
                Country = usa,
                Sex = Sex.Female,
                Data = new PlayerStats
                {
                    Rank = 10,
                    Points = 3521,
                    Weight = 72000,
                    Height = 175,
                    Age = 37,
                    Last = [0, 1, 1, 1, 0]
                }
            },
            new Player
            {
                FirstName = "Rafael",
                LastName = "Nadal",
                ShortName = "R.NAD",
                Picture = "https://tenisu.latelier.co/resources/Nadal.png",
                Country = esp,
                Sex = Sex.Male,
                Data = new PlayerStats
                {
                    Rank = 1,
                    Points = 1982,
                    Weight = 85000,
                    Height = 185,
                    Age = 33,
                    Last = [1, 0, 0, 0, 1]
                }
            }
        };

        await _context.Players.AddRangeAsync(players);
    }
}
