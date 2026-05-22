using ProductManagement.Api.Data;

namespace ProductManagement.Api.Utilities.Seeders.Interfaces
{
    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext context, IServiceProvider services);
    }
}