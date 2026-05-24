using Microsoft.AspNetCore.Cors.Infrastructure;

namespace ProductManagement.Api.Configurations;

public static class CorsConfiguration
{
    public static Action<CorsOptions> Configure()
    {
        return options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
            });
        };
    }
}