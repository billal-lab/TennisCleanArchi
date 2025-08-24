using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace TennisCleanArchi.Infrastructure.Auth
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _apiKey;

        public ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _apiKey = configuration["ApiKey:Value"]!;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (IsSwaggerRequest(context) || string.IsNullOrEmpty(_apiKey))
            {
                await _next(context);
                return;
            }
            if (!context.Request.Headers.TryGetValue("X-Api-Key", out var providedKey) || providedKey != _apiKey)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized: Invalid or missing API Key");
                return;
            }
            await _next(context);
        }

        private static bool IsSwaggerRequest(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();
            return path != null && (
                path.StartsWith("/swagger") ||
                path.StartsWith("/openapi") ||
                path.Contains("swagger.json") ||
                path.Contains("openapi.json")
            );
        }
    }
}
