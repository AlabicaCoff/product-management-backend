using ProductManagement.Api.Data;

namespace ProductManagement.Api.Utilities.Interfaces
{
    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext context, IServiceProvider services);
    }
}