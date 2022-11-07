using System.Security.Claims;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Infrastructure.Services.Auth;

public class ClaimsService : IClaimsService
{
    public ClaimsPrincipal AssignClaims(Guid userId, string email, string userRole)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, userRole),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        return new ClaimsPrincipal(claimsIdentity);
    }
}