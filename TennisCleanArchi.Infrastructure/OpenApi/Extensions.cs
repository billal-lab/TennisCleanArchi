using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace TennisCleanArchi.Infrastructure.OpenApi;

public static class Extensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", 
                new OpenApiInfo
                { 
                    Title = "Tennis Clean Archi API",
                    Version = "v1",
                    Contact = new OpenApiContact 
                    { 
                        Url = new Uri("https://www.linkedin.com/in/billal-benyoub-128038183"), Name = "Billal BENYOUB" 
                    } 
                }
            );
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tennis Clean Archi API V1");
        });

        return app;
    }
}
