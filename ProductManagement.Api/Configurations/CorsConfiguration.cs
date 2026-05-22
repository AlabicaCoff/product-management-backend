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
                policy.AllowAnyOrigin();
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });
        };
    }
}