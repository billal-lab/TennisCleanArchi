
using Microsoft.Extensions.DependencyInjection;
using TennisCleanArchi.Application.Common.Caching;
namespace TennisCleanArchi.Infrastructure.Caching;

public static class Extensions
{
	public static IServiceCollection AddCaching(this IServiceCollection services)
	{
		services.AddMemoryCache();
		services.AddScoped<ICachingService, CachingService>();
		return services;
	}
}
