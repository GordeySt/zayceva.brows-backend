using System.Security.Claims;

namespace Application.Common.Interfaces;

public interface IClaimsService
{
    ClaimsPrincipal AssignClaims(Guid userId, string email, string userRole);
}