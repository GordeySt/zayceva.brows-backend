using System.Security.Claims;

namespace Application.Common.Interfaces;

public interface IClaimsService
{
    Guid UserId { get; }
    
    ClaimsPrincipal AssignClaims(Guid userId, string email, string userRole);
}