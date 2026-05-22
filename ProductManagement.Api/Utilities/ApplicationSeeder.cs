using ProductManagement.Api.Data;
using ProductManagement.Api.Utilities.Interfaces;

namespace ProductManagement.Api.Utilities;
public class ApplicationSeeder
{
    private readonly IEnumerable<ISeeder> _seeders;
    public ApplicationSeeder(IEnumerable<ISeeder> seeders) => _seeders = seeders;

    public async Task SeedAllAsync(ApplicationDbContext context, IServiceProvider services)
    {
        foreach (var seeder in _seeders)
        {
            await seeder.SeedAsync(context, services);
        }
    }
}