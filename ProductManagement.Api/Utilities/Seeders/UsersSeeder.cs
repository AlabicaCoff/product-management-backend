using ProductManagement.Api.Data;
using ProductManagement.Api.Models;
using ProductManagement.Api.Utilities.Seeders.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ProductManagement.Api.Utilities.Seeders;
public class UsersSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext context, IServiceProvider services)
    {
        if (context.Users.Any()) return; 
        
         // Create a Admin User (password will be set separately)
        var adminUserId = "47d9b5c2-9aff-484d-87b9-e8ebe16a5868";
        var adminUser = new ApplicationUser()
        {
            Id = adminUserId,
            FirstName = "Admin",
            LastName = "User",
            Email = "Admin@ProductManagement.com",
            NormalizedEmail = "Admin@ProductManagement.com".ToUpper(),
            UserName = "Admin@ProductManagement.com",
            NormalizedUserName = "Admin@ProductManagement.com".ToUpper(),
            CreatedDate = new DateTime(2026, 5, 22, 12, 0, 0),
        };

        adminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(adminUser, "Admin@123");

        // Add Admin User to Database
        context.Users.Add(adminUser);
        await context.SaveChangesAsync();

        // Create a Non-Admin User (password will be set separately)
        var nonAdminUserId = "d9c8b5e2-9aff-484d-87b9-e8ebe16a5868";
        var nonAdminUser = new ApplicationUser()
        {
            Id = nonAdminUserId,
            FirstName = "Non-Admin",
            LastName = "User",
            Email = "NonAdmin@ProductManagement.com",
            NormalizedEmail = "NonAdmin@ProductManagement.com".ToUpper(),
            UserName = "NonAdmin@ProductManagement.com",
            NormalizedUserName = "NonAdmin@ProductManagement.com".ToUpper(),
            CreatedDate = new DateTime(2026, 5, 22, 12, 0, 0),
        };

        nonAdminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(nonAdminUser, "NonAdmin@123");

        // Add Non-Admin User to Database
        context.Users.Add(nonAdminUser);
        await context.SaveChangesAsync();
    }
}