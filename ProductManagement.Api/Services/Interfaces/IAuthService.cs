using ProductManagement.Api.DTOs;

namespace ProductManagement.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
