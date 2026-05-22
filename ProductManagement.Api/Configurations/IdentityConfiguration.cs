using Microsoft.AspNetCore.Identity;

namespace ProductManagement.Api.Configurations;

public static class IdentityConfiguration
{
    public static Action<IdentityOptions> Configure()
    {
        return options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        };
    }
}