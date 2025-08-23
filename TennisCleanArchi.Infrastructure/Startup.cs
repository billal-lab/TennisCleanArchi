using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TennisCleanArchi.Infrastructure.OpenApi;
using TennisCleanArchi.Infrastructure.Persistance;

namespace TennisCleanArchi.Infrastructure;

public static class Startup
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(
            options => options.UseInMemoryDatabase("TennisDatabase"));

        builder.Services.AddTransient<ApplicationSeeder>();

        builder.Services.AddSwagger();

        return builder;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseSwaggerUI();
        return app;
    }
}
