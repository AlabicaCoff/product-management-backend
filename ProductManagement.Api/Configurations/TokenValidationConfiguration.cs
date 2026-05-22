using Microsoft.IdentityModel.Tokens;

namespace ProductManagement.Api.Configurations;

public static class TokenValidationConfiguration
{
    public static Action<TokenValidationParameters> Configure(this WebApplicationBuilder builder)
    {
        return options =>
        {
            options.AuthenticationType = "Jwt";
            options.ValidateIssuer = true;
            options.ValidateAudience = true;
            options.ValidateLifetime = true;
            options.ValidateIssuerSigningKey = true;
            options.ValidIssuer = builder.Configuration["Jwt:Issuer"];
            options.ValidAudience = builder.Configuration["Jwt:Audience"];
            options.IssuerSigningKey =
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key configuration is missing.")));
        };
    }
}