using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ProductManagement.Api.Models;
using ProductManagement.Api.Services;

namespace ProductManagement.Tests.Services
{
    public class TokenServiceTests
    {
        private IConfiguration GetTestConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string?> {
                {"Jwt:Key", "ThisIsASecretKeyForJwtTokenGeneration"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            return configuration;
        }

        [Fact]
        public void TokenService_CreateJwtToken_ReturnsValidToken()
        {
            // Arrange
            var configuration = GetTestConfiguration();
            var service = new TokenService(configuration);
            var user = new ApplicationUser
            {
                UserName = "test@example.com",
                FirstName = "Test",
                LastName = "User"
            };
            var roles = new List<string> { "Admin" };

            // Act
            var token = service.CreateJwtToken(user, roles);

            // Assert
            token.Should().NotBeNullOrEmpty();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            jwtToken.Should().NotBeNull();
            jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value.Should().Be(user.UserName);
            jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).Should().BeEquivalentTo(roles);
        }

        [Fact]
        public void TokenService_CreateJwtToken_NullUser_ThrowsArgumentException()
        {
            // Arrange
            var configuration = GetTestConfiguration();
            var service = new TokenService(configuration);
            ApplicationUser user = null!;
            var roles = new List<string> { "Admin" };

            // Act
            Action act = () => service.CreateJwtToken(user, roles);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName("user");
        }

        [Fact]
        public void TokenService_CreateJwtToken_NullUserNameInUser_ThrowsArgumentException()
        {
            // Arrange
            var configuration = GetTestConfiguration();
            var service = new TokenService(configuration);
            var user = new ApplicationUser
            {
                UserName = null,
                FirstName = "Test",
                LastName = "User"
            };
            var roles = new List<string> { "Admin" };

            // Act
            Action act = () => service.CreateJwtToken(user, roles);

            // Assert
            act.Should().Throw<ArgumentException>().WithParameterName("user.UserName");
        }

        [Fact]
        public void TokenService_CreateJwtToken_EmptyUserNameInUser_ThrowsArgumentException()
        {
            // Arrange
            var configuration = GetTestConfiguration();
            var service = new TokenService(configuration);
            var user = new ApplicationUser
            {
                UserName = "",
                FirstName = "Test",
                LastName = "User"
            };
            var roles = new List<string> { "Admin" };

            // Act
            Action act = () => service.CreateJwtToken(user, roles);

            // Assert
            act.Should().Throw<ArgumentException>().WithParameterName("user.UserName");
        }
    }
}