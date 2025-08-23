using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using TennisCleanArchi.Infrastructure.OpenApi;

namespace TennisCleanArchi.Infrastructure;

public static class Startup
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Services.AddSwagger();
        return applicationBuilder;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseSwaggerUI();
        return app;
    }
}
