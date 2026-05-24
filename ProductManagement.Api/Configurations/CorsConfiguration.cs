using Microsoft.AspNetCore.Cors.Infrastructure;

namespace ProductManagement.Api.Configurations;

public static class CorsConfiguration
{
    public static Action<CorsOptions> Configure(IConfiguration configuration)
    {
        return options =>
        {
            var allowedOrigins = configuration["Cors:AllowedOrigins"]?.Split(',') ?? new[] { "http://localhost:4200" };

            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
            });
        };
    }
}