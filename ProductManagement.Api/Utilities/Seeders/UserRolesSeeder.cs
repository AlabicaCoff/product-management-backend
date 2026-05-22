using ProductManagement.Api.Data;
using ProductManagement.Api.Models;
using ProductManagement.Api.Utilities.Seeders.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ProductManagement.Api.Utilities.Seeders;
public class UserRolesSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext context, IServiceProvider services)
    {
        if (context.UserRoles.Any()) return; 
        
        var adminRoleId = "9ab55ee9-52a9-45c9-ae26-1013c376b383";
        var nonAdminRoleId = "2f3acff7-944b-4c05-83dc-a4175d93a018";

        var adminUserId = "47d9b5c2-9aff-484d-87b9-e8ebe16a5868";
        var nonAdminUserId = "d9c8b5e2-9aff-484d-87b9-e8ebe16a5868";

        var userRoles = new List<IdentityUserRole<string>>
        {
            new IdentityUserRole<string>()
            {
                UserId = adminUserId,
                RoleId = adminRoleId
            },
            new IdentityUserRole<string>()
            {
                UserId = nonAdminUserId,
                RoleId = nonAdminRoleId
            }
        };

        // Seed the user roles
        context.UserRoles.AddRange(userRoles);
        await context.SaveChangesAsync();
    }
}