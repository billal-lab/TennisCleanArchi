using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TennisCleanArchi.Application.Data;
using TennisCleanArchi.Infrastructure.Exceptions;
using TennisCleanArchi.Infrastructure.OpenApi;
using TennisCleanArchi.Infrastructure.Persistance;
using TennisCleanArchi.Infrastructure.Validations;

namespace TennisCleanArchi.Infrastructure;

public static class Startup
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(
            options => options.UseInMemoryDatabase("TennisDatabase"));

        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        builder.Services.AddTransient<ApplicationSeeder>();

        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        builder.Services.AddSwagger();

        return builder;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseSwaggerUI();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}
