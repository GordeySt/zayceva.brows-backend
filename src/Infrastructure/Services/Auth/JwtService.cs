using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces;
using Infrastructure.Settings;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.Auth;

public class JwtService : IJwtService
{
    private readonly IdentitySettings _identitySettings;

    public JwtService(IdentitySettings identitySettings)
    {
        _identitySettings = identitySettings;
    }

    public string GenerateJwtToken(IEnumerable<Claim> claims)
    {
        var key = CreateSecuritySigningKey(_identitySettings.JwtSettings.IssuerSigningKey);
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        
        var tokenExpirationTime = TimeSpan.FromMinutes(_identitySettings.JwtSettings.ExpirationMinutes);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _identitySettings.JwtSettings.ValidIssuer,
            audience: _identitySettings.JwtSettings.ValidAudience,
            expires: DateTime.UtcNow.Add(tokenExpirationTime),
            claims: claims,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
    
    public static SymmetricSecurityKey CreateSecuritySigningKey(string key) =>
        new (Encoding.UTF8.GetBytes(key));
}