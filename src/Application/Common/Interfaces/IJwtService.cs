using System.Security.Claims;

namespace Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(IEnumerable<Claim> claims);
}