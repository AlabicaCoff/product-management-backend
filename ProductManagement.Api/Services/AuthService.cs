using Microsoft.AspNetCore.Identity;
using ProductManagement.Api.Services.Interfaces;
using ProductManagement.Api.DTOs;
using ProductManagement.Api.Models;
using Microsoft.EntityFrameworkCore;


namespace ProductManagement.Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        this._userManager = userManager;
        this._tokenService = tokenService;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        // Check Username
        var identityUser = await _userManager.FindByNameAsync(request.UserName);

        if (identityUser is not null)
        {
            // Check Password
            var checkPasswordResult = await _userManager.CheckPasswordAsync(identityUser, request.Password);

            if (checkPasswordResult)
            {
                // Get the User from Database
                var userData = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == identityUser.Id);
                if (userData is null)
                {
                    return new LoginResponseDto
                    {
                        IsLoginSuccess = false,
                        FailureMessage = "Invalid username or password."
                    };
                }

                var roles = await _userManager.GetRolesAsync(identityUser);
                var jwtToken = _tokenService.CreateJwtToken(identityUser, roles.ToList());
                var response = new LoginResponseDto()
                {
                    IsLoginSuccess = true,
                    UserName = request.UserName,
                    Roles = roles.ToList(),
                    Token = jwtToken
                };
                return response;
            }

            return new LoginResponseDto
            {
                IsLoginSuccess = false,
                FailureMessage = "Invalid username or password."
            };
        }
        return new LoginResponseDto
        {
            IsLoginSuccess = false,
            FailureMessage = "Invalid username or password."
        };
    }
}