using ProductManagement.Api.Models;

namespace ProductManagement.Api.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateJwtToken(ApplicationUser user, List<string> roles);
    }
}
