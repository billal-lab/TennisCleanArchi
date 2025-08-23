using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TennisCleanArchi.Application.Players;

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

        // Mapster configuration: player to player dto
        TypeAdapterConfig<Domain.Player, PlayerDto>.NewConfig()
            .Map(dest => dest.Sex, src => src.Sex.Value);

        return services;
    }
}
