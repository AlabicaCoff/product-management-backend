using Microsoft.AspNetCore.Identity;
using ProductManagement.Api.Services;
using ProductManagement.Api.Services.Interfaces;
using ProductManagement.Api.DTOs;
using ProductManagement.Api.Models;
using Microsoft.EntityFrameworkCore;
using FakeItEasy;
using FluentAssertions;

namespace ProductManagement.Tests.Services;

public class AuthServiceTests
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthServiceTests()
    {
        var store = A.Fake<IUserStore<ApplicationUser>>();
        this._userManager = A.Fake<UserManager<ApplicationUser>>(x => 
            x.WithArgumentsForConstructor(() => new UserManager<ApplicationUser>(store, null!, null!, null!, null!, null!, null!, null!, null!)));
        this._tokenService = A.Fake<ITokenService>();
    }

    [Fact]
    public async Task AuthService_LoginAsync_ValidUserAndValidPassword_ReturnsLoginResponseDto()
    {
        // Arrange
        var loginRequest = new LoginRequestDto
        {
            UserName = "test@example.com",
            Password = "password"
        };

        var loginResponse = new LoginResponseDto
        {
            IsLoginSuccess = true,
            UserName = "test@example.com",
            Roles = new List<string> { "Admin" },
            Token = "jwtToken"
        };
        
        var identityUser = new ApplicationUser
        {
            Id = "1",
            UserName = "test@example.com",
            FirstName = "Test",
            LastName = "User"
        };

        var roles = new List<string> { "Admin" };

        A.CallTo(() => _userManager.FindByNameAsync(loginRequest.UserName)).Returns(Task.FromResult<ApplicationUser?>(identityUser));
        A.CallTo(() => _userManager.CheckPasswordAsync(identityUser, loginRequest.Password)).Returns(Task.FromResult(true));
        A.CallTo(() => _userManager.FindByIdAsync(identityUser.Id)).Returns(Task.FromResult<ApplicationUser?>(identityUser));
        A.CallTo(() => _userManager.GetRolesAsync(identityUser)).Returns(Task.FromResult<IList<string>>(roles));
        A.CallTo(() => _tokenService.CreateJwtToken(identityUser, roles)).Returns(loginResponse.Token);

        var service = new AuthService(_userManager, _tokenService);

        // Act
        var result = await service.LoginAsync(loginRequest);

        // Assert
        result.Should().NotBeNull();
        result.IsLoginSuccess.Should().BeTrue();
        result.UserName.Should().Be(loginRequest.UserName);
        result.Roles.Should().BeEquivalentTo(roles);
        result.Token.Should().Be(loginResponse.Token);
    }

    [Fact]
    public async Task AuthService_LoginAsync_InvalidUsername_ReturnsLoginResponseDto()
    {
        // Arrange
        var loginRequest = new LoginRequestDto
        {
            UserName = "wrongUsername@example.com",
            Password = "password"
        };

        var loginResponse = new LoginResponseDto
        {
            IsLoginSuccess = false,
            FailureMessage = "Invalid username or password."
        };

        A.CallTo(() => _userManager.FindByNameAsync(loginRequest.UserName)).Returns(Task.FromResult<ApplicationUser?>(null));

        var service = new AuthService(_userManager, _tokenService);

        // Act
        var result = await service.LoginAsync(loginRequest);

        // Assert
        result.Should().NotBeNull();
        result.IsLoginSuccess.Should().BeFalse();
        result.FailureMessage.Should().Be(loginResponse.FailureMessage);
    }

    [Fact]
    public async Task AuthService_LoginAsync_InvalidPassword_ReturnsLoginResponseDto()
    {
        // Arrange
        var loginRequest = new LoginRequestDto
        {
            UserName = "test@example.com",
            Password = "wrongPassword"
        };

        var loginResponse = new LoginResponseDto
        {
            IsLoginSuccess = false,
            FailureMessage = "Invalid username or password."
        };

        var identityUser = new ApplicationUser
        {
            Id = "1",
            UserName = "test@example.com",
            FirstName = "Test",
            LastName = "User"
        };

        A.CallTo(() => _userManager.FindByNameAsync(loginRequest.UserName)).Returns(Task.FromResult<ApplicationUser?>(identityUser));
        A.CallTo(() => _userManager.CheckPasswordAsync(identityUser, loginRequest.Password)).Returns(Task.FromResult(false));

        var service = new AuthService(_userManager, _tokenService);

        // Act
        var result = await service.LoginAsync(loginRequest);

        // Assert
        result.Should().NotBeNull();
        result.IsLoginSuccess.Should().BeFalse();
        result.FailureMessage.Should().Be(loginResponse.FailureMessage);
    }
    
}