using ProductManagement.Api.Data;
using ProductManagement.Api.Models;
using ProductManagement.Api.Utilities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ProductManagement.Api.Utilities;
public class RolesSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext context, IServiceProvider services)
    {
        if (context.Roles.Any()) return; 
        
        var adminRoleId = "9ab55ee9-52a9-45c9-ae26-1013c376b383";
        var nonAdminRoleId = "2f3acff7-944b-4c05-83dc-a4175d93a018";

        // Create Roles
        var roles = new List<IdentityRole>
        {
            new IdentityRole()
            {
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "Admin".ToUpper(),
                ConcurrencyStamp = adminRoleId
            },
            new IdentityRole()
            {
                Id = nonAdminRoleId,
                Name = "Non-Admin",
                NormalizedName = "Non-Admin".ToUpper(),
                ConcurrencyStamp = nonAdminRoleId
            }
        };

        // Seed the roles
        context.Roles.AddRange(roles);
        await context.SaveChangesAsync();
    }
}