using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TennisCleanArchi.Shared;

namespace TennisCleanArchi.Application;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // add fluent validation
        services.AddValidatorsFromAssembly(assembly);

        // add mediatr
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        // mapster : string - sex mapping
        TypeAdapterConfig<string, Sex>.NewConfig()
            .MapWith(value => Sex.FromValue(value));

        // mapster : sex - string mapping
        TypeAdapterConfig<Sex, string>.NewConfig()
            .MapWith(value => value.Value);

        return services;
    }
}
