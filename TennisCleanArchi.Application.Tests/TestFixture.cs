using Mapster;
using Microsoft.EntityFrameworkCore;
using TennisCleanArchi.Application.Data;
using TennisCleanArchi.Infrastructure.Persistance;
using TennisCleanArchi.Shared;

namespace TennisCleanArchi.Application.Tests;

public class TestFixture
{
    public TestFixture()
    {
        // mapster : string - sex mapping
        TypeAdapterConfig<string, Sex>.NewConfig()
            .MapWith(value => Sex.FromValue(value));

        // mapster : sex - string mapping
        TypeAdapterConfig<Sex, string>.NewConfig()
            .MapWith(value => value.Value);
    }

    public IApplicationDbContext DbContext
    {
        get
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase_" + Guid.NewGuid())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
