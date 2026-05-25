using ProductManagement.Api.Services.Interfaces;
using ProductManagement.Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductManagement.Api.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateJwtToken(ApplicationUser user, List<string> roles)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrEmpty(user.UserName);

        // Create Claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var jwtKey = _configuration["Jwt:Key"];
        var jwtIssuer = _configuration["Jwt:Issuer"];
        var jwtAudience = _configuration["Jwt:Audience"];

        if (jwtKey == null || jwtIssuer == null || jwtAudience == null)
        {
            throw new InvalidOperationException("JWT configuration is missing");
        }
        
        // JWT Security Token Parameters
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        // Return Token
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}